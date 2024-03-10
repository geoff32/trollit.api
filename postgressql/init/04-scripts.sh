#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    BEGIN;
        CREATE TYPE app.scriptid AS ENUM (
            'profile',
            'effect',
            'view',
            'equipment',
            'flies'
        );

        CREATE TABLE app.scriptcategories (
            id UUID PRIMARY KEY,
            name VARCHAR(20) NOT NULL,
            maxcall INT NOT NULL
        );

        CREATE TABLE app.scriptinfos (
            id app.scriptid PRIMARY KEY,
            name VARCHAR(20) NOT NULL,
            script VARCHAR(100) NOT NULL,
            categoryid UUID NOT NULL REFERENCES app.scriptcategories(id),
            UNIQUE(script)
        );

        CREATE TABLE app.scriptsettings (
            scriptid app.scriptid PRIMARY KEY,
            trollid INT NOT NULL REFERENCES app.trollaccounts(id),
            maxcall INT NULL,
            UNIQUE(scriptid, trollid)
        );

        CREATE TABLE app.scriptshistory (
            scriptid app.scriptid,
            trollid INT NOT NULL,
            date TIMESTAMPTZ NOT NULL
        );

        CREATE TYPE app.scriptcategory AS (
            name VARCHAR(20),
            maxcall INT
        );

        CREATE TYPE app.scriptinfo AS (
            id app.scriptid,
            name VARCHAR(20),
            script VARCHAR(100),
            category app.scriptcategory
        );

        CREATE TYPE app.scriptcounter AS (
            script app.scriptinfo,
            call INT,
            maxcall INT
        );

        CREATE TYPE app.trollscripts AS (
            trollid INT,
            trollname VARCHAR(100),
            profile app.scriptcounter,
            effect app.scriptcounter,
            view app.scriptcounter,
            equipment app.scriptcounter,
            flies app.scriptcounter
        );

        CREATE OR REPLACE PROCEDURE app.add_scriptshistory(pTrollId INT, pScriptId app.scriptid, pDate TIMESTAMPTZ)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            INSERT INTO app.scriptshistory(scriptid, trollid, date)
            VALUES (pScriptId, pTrollId, pDate);
        END
        \$\$ SECURITY DEFINER;

        CREATE OR REPLACE PROCEDURE app.clean_scriptshistory(pBeforeDate TIMESTAMPTZ)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            DELETE FROM app.scriptshistory WHERE date < pBeforeDate;
        END
        \$\$ SECURITY DEFINER;

        CREATE OR REPLACE PROCEDURE app.update_script_settings(pAccountId UUID, pScriptId app.scriptid, pMaxCall INT)
        LANGUAGE plpgsql AS
        \$\$
        BEGIN
            INSERT INTO app.scriptsettings(scriptid, trollid, maxcall)
            SELECT pScriptId, a.trollid, pMaxCall
            FROM app.trollaccounts a
            WHERE a.accountid = pAccountId
            ON CONFLICT(scriptid, trollid) DO UPDATE SET calllimit = excluded.calllimit;
        END
        \$\$ SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_script_category(pCategoryId UUID)
        RETURNS setof app.scriptcategory AS
        \$\$
            SELECT c.name, c.maxcall
            FROM app.scriptcategories c
            WHERE c.id = pCategoryid;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_script_maxcall(pTrollId INT, pScriptId app.scriptid)
        RETURNS INT AS
        \$\$
            SELECT s.maxcall
            FROM app.scriptsettings s
            WHERE s.trollid = pTrollId AND s.scriptid = pScriptId;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.count_script_usage(pTrollId INT, pScriptId app.scriptid, pFromDate TIMESTAMPTZ)
        RETURNS INT AS
        \$\$
            SELECT COUNT(s.date)
            FROM app.scriptshistory s
            WHERE s.trollid = pTrollId
            AND s.scriptid = pScriptId
            AND s.date >= pFromDate;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_script_counter(pTrollId INT, pFromDate TIMESTAMPTZ, pScriptId app.scriptid)
        RETURNS setof app.scriptcounter AS
        \$\$
            SELECT (s.id, s.name, s.script, app.get_script_category(s.categoryid))::app.scriptinfo, app.count_script_usage(pTrollId, s.id, pFromDate) as call, app.get_script_maxcall(pTrollId, s.id) as maxcall
            FROM app.scriptinfos s
            WHERE s.id = pScriptId;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_trollscripts(pTrollId INT, pFromDate TIMESTAMPTZ)
        RETURNS setof app.trollscripts AS
        \$\$
            SELECT t.id as trollid, t.name as trollname,
                app.get_script_counter(t.id, pFromDate, 'profile') AS profile,
                app.get_script_counter(t.id, pFromDate, 'effect') AS effect,
                app.get_script_counter(t.id, pFromDate, 'view') AS view,
                app.get_script_counter(t.id, pFromDate, 'equipment') AS equipment,
                app.get_script_counter(t.id, pFromDate, 'flies') AS flies
            FROM app.trolls t
            WHERE t.id = pTrollId
            GROUP BY t.id, t.name;
        \$\$  LANGUAGE sql SECURITY DEFINER;

        CREATE OR REPLACE FUNCTION app.get_scriptinfos()
        RETURNS setof app.scriptinfo AS
        \$\$
            SELECT s.id, s.name, s.script, app.get_script_category(s.categoryid)
            FROM app.scriptinfos s;
        \$\$  LANGUAGE sql SECURITY DEFINER;
        
        INSERT INTO app.scriptcategories
            (id, name, maxcall)
        VALUES
            ('99879c6e-4162-4495-81e7-e12d14c012c3', 'Scripts dynamiques', 24),
            ('204cb063-0265-49e4-bd77-3cd2ff7d50d6', 'Scripts statiques', 10);
        
        INSERT INTO app.scriptinfos
            (id, name, script, categoryid)
        VALUES
            ('profile', 'Profil', '/SP_Profil4.php', '99879c6e-4162-4495-81e7-e12d14c012c3'),
            ('effect', 'Bonus Malus', '/SP_Bonusmalus.php', '99879c6e-4162-4495-81e7-e12d14c012c3'),
            ('view', 'Vue', '/SP_Vue2.php', '99879c6e-4162-4495-81e7-e12d14c012c3'),
            ('equipment', 'Equipement', '/SP_Equipement.php', '204cb063-0265-49e4-bd77-3cd2ff7d50d6'),
            ('flies', 'Mouches', '/SP_Mouche.php', '204cb063-0265-49e4-bd77-3cd2ff7d50d6');
    COMMIT;
EOSQL