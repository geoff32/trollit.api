#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    BEGIN;
        CREATE TYPE app.sharestatus AS ENUM (
            'owner',
            'admin',
            'user',
            'guest'
        );

        CREATE TYPE app.featurestatus AS ENUM (
            'inactive',
            'read',
            'readwrite'
        );

        CREATE TYPE app.featureid AS ENUM (
            'profile',
            'view'
        );

        CREATE TABLE app.sharepolicies (
            id UUID PRIMARY KEY,
            name VARCHAR(50) NOT NULL
        );

        CREATE TABLE app.trollpolicies (
            sharepolicyid UUID NOT NULL REFERENCES app.sharepolicies(id),
            trollid INT NOT NULL,
            status app.sharestatus NOT NULL,
            UNIQUE(sharepolicyid, trollid)
        );

        CREATE TABLE app.trollpoliciesfeatures (
            sharepolicyid UUID NOT NULL REFERENCES app.sharepolicies(id),
            trollid INT NOT NULL,
            featureid app.featureid NOT NULL,
            status app.featurestatus NOT NULL,
            UNIQUE(sharepolicyid, trollid, featureid)
        );

        CREATE TYPE app.trollfeature AS (
            id app.featureid,
            status app.featurestatus
        );

        CREATE TYPE app.trollshare AS (
            trollid INT,
            status app.sharestatus,
            features app.trollfeature[]
        );

        CREATE TYPE app.sharepolicy AS (
            id UUID,
            name VARCHAR(50),
            trolls app.trollshare[]
        );

        CREATE OR REPLACE PROCEDURE app.update_sharepolicy(pSharePolicy app.sharepolicy)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            INSERT INTO app.sharepolicies(id, name)
            VALUES(pSharePolicy.id, pSharePolicy.name)
            ON CONFLICT(id) DO UPDATE SET name = excluded.name;

            INSERT INTO app.trollpolicies(sharepolicyid, trollid, status)
            SELECT pSharePolicy.id, t.trollid, t.status
            FROM UNNEST(pSharePolicy.trolls) t
            ON CONFLICT(sharepolicyid, trollid) DO UPDATE SET status = excluded.status;

            INSERT INTO app.trollpoliciesfeatures(sharepolicyid, trollid, featureid, status)
            SELECT pSharePolicy.id, t.trollid, f.id, f.status
            FROM UNNEST(pSharePolicy.trolls) t, UNNEST(t.features) f
            ON CONFLICT(sharepolicyid, trollid, featureid) DO UPDATE SET status = excluded.status;
        END
        \$\$ SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_sharefeatures(pSharePolicyId UUID, pTrollId INT)
        RETURNS app.trollfeature[] AS
        \$\$
            SELECT array_agg((f.featureid, f.status)::app.trollfeature)
            FROM app.trollpoliciesfeatures f
            WHERE f.sharepolicyid = pSharePolicyId AND f.trollid = pTrollId;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_trollshares(pSharePolicyId UUID)
        RETURNS app.trollshare[] AS
        \$\$
            SELECT array_agg((t.trollid, t.status, app.get_sharefeatures(t.sharepolicyid, t.trollid))::app.trollshare)
            FROM app.trollpolicies t
            WHERE t.sharepolicyid = pSharePolicyId;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_sharepolicy(pSharePolicyId UUID)
        RETURNS setof app.sharepolicy AS
        \$\$
            SELECT s.id, s.name, app.get_trollshares(s.id)
            FROM app.sharepolicies s
            LEFT JOIN app.trollpolicies t ON t.sharepolicyid = s.id
            LEFT JOIN app.trollpoliciesfeatures f ON f.sharepolicyid = s.id AND f.trollid = t.trollid
            WHERE s.id = pSharePolicyId
            GROUP BY s.id;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_trollsharepolicies(pTrollId INT)
        RETURNS setof app.sharepolicy AS
        \$\$
            SELECT app.get_sharepolicy(s.id)
            FROM app.sharepolicies s
            INNER JOIN app.trollpolicies t ON t.sharepolicyid = s.id
            WHERE t.trollid = pTrollId;
        \$\$  LANGUAGE sql SECURITY DEFINER;


        CREATE OR REPLACE PROCEDURE app.delete_sharepolicy(pSharePolicyId UUID)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            DELETE FROM app.trollpoliciesfeatures
            WHERE sharepolicyid = pSharePolicyId;

            DELETE FROM app.trollpolicies
            WHERE sharepolicyid = pSharePolicyId;

            DELETE FROM app.sharepolicies
            WHERE id = pSharePolicyId;
        END
        \$\$ SECURITY DEFINER;
    COMMIT;
EOSQL