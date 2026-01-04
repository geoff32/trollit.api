BEGIN;

    CREATE TYPE app.policystatus AS ENUM (
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
    
    CREATE TABLE app.policies (
        id UUID PRIMARY KEY,
        name VARCHAR(50) NOT NULL
    );
    
    CREATE TABLE app.trollpolicies (
        policyid UUID NOT NULL REFERENCES app.policies(id),
        trollid INT NOT NULL,
        status app.policystatus NOT NULL,
        UNIQUE(policyid, trollid)
    );
    
    CREATE TABLE app.trollpoliciesfeatures (
        policyid UUID NOT NULL REFERENCES app.policies(id),
        trollid INT NOT NULL,
        featureid app.featureid NOT NULL,
        status app.featurestatus NOT NULL,
        UNIQUE(policyid, trollid, featureid)
    );
    
    CREATE TYPE app.trollfeature AS (
        id app.featureid,
        status app.featurestatus
    );
    
    CREATE TYPE app.trollshare AS (
        trollid INT,
        status app.policystatus,
        features app.trollfeature[]
    );
    
    CREATE TYPE app.policy AS (
        id UUID,
        name VARCHAR(50),
        trolls app.trollshare[]
    );
    
    CREATE OR REPLACE PROCEDURE app.update_policy(pPolicy app.policy)
    LANGUAGE plpgsql AS
    $$
    BEGIN
        INSERT INTO app.policies(id, name)
        VALUES(pPolicy.id, pPolicy.name)
            ON CONFLICT(id) DO UPDATE SET name = excluded.name;
        
        INSERT INTO app.trollpolicies(policyid, trollid, status)
        SELECT pPolicy.id, t.trollid, t.status
        FROM UNNEST(pPolicy.trolls) t
            ON CONFLICT(policyid, trollid) DO UPDATE SET status = excluded.status;
        
        INSERT INTO app.trollpoliciesfeatures(policyid, trollid, featureid, status)
        SELECT pPolicy.id, t.trollid, f.id, f.status
        FROM UNNEST(pPolicy.trolls) t, UNNEST(t.features) f
        ON CONFLICT(policyid, trollid, featureid) DO UPDATE SET status = excluded.status;
    END
    $$ SECURITY DEFINER;
    
    CREATE OR REPLACE FUNCTION app.get_sharefeatures(pPolicyId UUID, pTrollId INT)
    RETURNS app.trollfeature[] AS
    $$
        SELECT array_agg((f.featureid, f.status)::app.trollfeature)
        FROM app.trollpoliciesfeatures f
        WHERE f.policyid = pPolicyId AND f.trollid = pTrollId;
    $$  LANGUAGE sql SECURITY DEFINER;
    
    CREATE OR REPLACE FUNCTION app.get_trollshares(pPolicyId UUID)
    RETURNS app.trollshare[] AS
    $$
        SELECT array_agg((t.trollid, t.status, app.get_sharefeatures(t.policyid, t.trollid))::app.trollshare)
        FROM app.trollpolicies t
        WHERE t.policyid = pPolicyId;
    $$  LANGUAGE sql SECURITY DEFINER;
    
    CREATE OR REPLACE FUNCTION app.get_policy(pPolicyId UUID)
    RETURNS setof app.policy AS
    $$
        SELECT s.id, s.name, app.get_trollshares(s.id)
        FROM app.policies s
        LEFT JOIN app.trollpolicies t ON t.policyid = s.id
        LEFT JOIN app.trollpoliciesfeatures f ON f.policyid = s.id AND f.trollid = t.trollid
        WHERE s.id = pPolicyId
        GROUP BY s.id;
    $$  LANGUAGE sql SECURITY DEFINER;
    
    CREATE OR REPLACE FUNCTION app.get_trollpolicies(pTrollId INT)
    RETURNS setof app.policy AS
    $$
        SELECT app.get_policy(s.id)
        FROM app.policies s
        INNER JOIN app.trollpolicies t ON t.policyid = s.id
        WHERE t.trollid = pTrollId;
    $$  LANGUAGE sql SECURITY DEFINER;
    
    
    CREATE OR REPLACE PROCEDURE app.delete_policy(pPolicyId UUID)
    LANGUAGE plpgsql AS
    $$
    BEGIN
        DELETE FROM app.trollpoliciesfeatures
        WHERE policyid = pPolicyId;
        
        DELETE FROM app.trollpolicies
        WHERE policyid = pPolicyId;
        
        DELETE FROM app.policies
        WHERE id = pPolicyId;
    END
    $$ SECURITY DEFINER;
       
COMMIT;