#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    BEGIN;
        CREATE TYPE app.scripttypes AS ENUM (
            'profile',
            'effect',
            'view',
            'equipment'
        );

        CREATE TABLE app.scriptcategories (
            id UUID PRIMARY KEY,
            name VARCHAR(20) NOT NULL,
            calllimit INT NOT NULL
        );

        CREATE TABLE app.scriptinfos (
            id UUID PRIMARY KEY,
            script VARCHAR(100) NOT NULL,
            type app.scripttypes NOT NULL,
            categoryid UUID NOT NULL REFERENCES app.scriptcategories(id),
            UNIQUE(script)
        );

        CREATE TABLE app.scriptshistory (
            id UUID NOT NULL REFERENCES app.scriptinfos(id),
            trollid INT NOT NULL,
            date TIMESTAMPTZ NOT NULL
        );

        CREATE TYPE app.attributes AS ENUM (
            'vitality',
            'view',
            'attack',
            'dodge',
            'damage',
            'regeneration',
            'armor',
            'magicmastery',
            'magicresistance',
            'turn'
        );

        CREATE TABLE app.trollattributes (
            trollid INT NOT NULL,
            attribute app.attributes NOT NULL,
            value INT NOT NULL,
            physicalbonus INT NOT NULL,
            magicalbonus INT NOT NULL,
            UNIQUE(trollid, attribute)
        );

        CREATE TABLE app.trolls (
            id INT PRIMARY KEY,
            name VARCHAR(100) NOT NULL,
            breed VARCHAR(30) NOT NULL,
            shortbreed VARCHAR(1) NOT NULL,
            level INT NOT NULL
        );


        CREATE TYPE app.attribute AS (
            value INT,
            physicalbonus INT,
            magicalbonus INT
        );

        CREATE TYPE app.profile AS (
            vitality app.attribute,
            view app.attribute,
            attack app.attribute,
            dodge app.attribute,
            damage app.attribute,
            regeneration app.attribute,
            armor app.attribute,
            magicmastery app.attribute,
            magicresistance app.attribute,
            turnduration app.attribute
        );

        CREATE TYPE app.troll AS (
            id INT,
            name VARCHAR(100),
            breed VARCHAR(30),
            shortbreed VARCHAR(1),
            level INT,
            profile app.profile
        );

        CREATE OR REPLACE PROCEDURE app.add_scriptshistory(pTrollId INT, pScript VARCHAR(100), pDate TIMESTAMPTZ)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            INSERT INTO app.scriptshistory
            (id, trollid, date)
            SELECT s.id, pTrollId, pDate
            FROM app.scriptinfos s
            WHERE s.script = pScript;
        END
        \$\$ SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_attribute(pId INT, pAttribute app.attributes)
        RETURNS app.attribute AS
        \$\$
            SELECT value, physicalbonus, magicalbonus
            FROM app.trollattributes
            WHERE trollid = pId AND attribute = pAttribute;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_troll(pId INT)
        RETURNS setof app.troll AS
        \$\$
            SELECT
                t.id, t.name, t.breed, t.shortbreed, t.level,
                (
                    app.get_attribute(t.id, 'vitality'),
                    app.get_attribute(t.id, 'view'),
                    app.get_attribute(t.id, 'attack'),
                    app.get_attribute(t.id, 'dodge'),
                    app.get_attribute(t.id, 'damage'),
                    app.get_attribute(t.id, 'regeneration'),
                    app.get_attribute(t.id, 'armor'),
                    app.get_attribute(t.id, 'magicmastery'),
                    app.get_attribute(t.id, 'magicresistance'),
                    app.get_attribute(t.id, 'turn')
                )::app.profile
            FROM app.trolls t
            WHERE t.id = pId;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE PROCEDURE app.refresh_attribute(pTrollId INT, pAttribute app.attributes, pAttributeValue app.attribute)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            INSERT INTO app.trollattributes(trollid, attribute, value, physicalbonus, magicalbonus)
            VALUES(pTrollId, pAttribute, pAttributeValue.value, pAttributeValue.physicalbonus, pAttributeValue.magicalbonus)
            ON CONFLICT (trollid, attribute) DO UPDATE SET value = excluded.value, physicalbonus = excluded.physicalbonus, magicalbonus = excluded.magicalbonus;
        END
        \$\$ SECURITY DEFINER;

        CREATE OR REPLACE PROCEDURE app.refresh_troll(pTroll app.troll)
        LANGUAGE plpgsql AS
        \$\$
        DECLARE
            v_profile app.profile;
        BEGIN
            INSERT INTO app.trolls(id, name, breed, shortbreed, level)
            VALUES(pTroll.id, pTroll.name, pTroll.breed, pTroll.shortbreed, pTroll.level)
            ON CONFLICT (id) DO UPDATE SET name = excluded.name, breed = excluded.breed, shortbreed = excluded.shortbreed, level = excluded.level;

            v_profile := pTroll.profile;

            CALL app.refresh_attribute(pTroll.id, 'vitality', v_profile.vitality);
            CALL app.refresh_attribute(pTroll.id, 'view', v_profile.view);
            CALL app.refresh_attribute(pTroll.id, 'dodge', v_profile.dodge);
            CALL app.refresh_attribute(pTroll.id, 'damage', v_profile.damage);
            CALL app.refresh_attribute(pTroll.id, 'regeneration', v_profile.regeneration);
            CALL app.refresh_attribute(pTroll.id, 'armor', v_profile.armor);
            CALL app.refresh_attribute(pTroll.id, 'magicmastery', v_profile.magicmastery);
            CALL app.refresh_attribute(pTroll.id, 'magicresistance', v_profile.magicresistance);
            CALL app.refresh_attribute(pTroll.id, 'turn', v_profile.turnduration);
        END
        \$\$ SECURITY DEFINER;
        
        INSERT INTO app.scriptcategories
            (id, name, calllimit)
        VALUES
            ('99879c6e-4162-4495-81e7-e12d14c012c3', 'Scripts dynamiques', 24),
            ('204cb063-0265-49e4-bd77-3cd2ff7d50d6', 'Scripts statiques', 10);
        
        INSERT INTO app.scriptinfos
            (id, script, type, categoryid)
        VALUES
            ('d786bbb7-c056-41ff-9e02-d13799b21eae', '/SP_Profil4.php', 'profile', '99879c6e-4162-4495-81e7-e12d14c012c3'),
            ('b9c1253a-3d8b-4cd1-b6da-6a096aa7c9e4', '/SP_Bonusmalus.php', 'effect', '99879c6e-4162-4495-81e7-e12d14c012c3'),
            ('488f62db-3a87-441f-8507-4d56b658d90b', '/SP_Vue2.php', 'view', '99879c6e-4162-4495-81e7-e12d14c012c3'),
            ('876a46c8-06d6-4dee-9826-c2046157e4a0', '/SP_Equipement.php', 'equipment', '204cb063-0265-49e4-bd77-3cd2ff7d50d6');
    COMMIT;
EOSQL