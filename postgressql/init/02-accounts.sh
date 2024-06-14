#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE TABLE app.accounts (
        id UUID PRIMARY KEY,
        login VARCHAR(100) UNIQUE NOT NULL,
        password BYTEA NOT NULL
    );
    CREATE TABLE app.trollaccounts (
        id INT PRIMARY KEY,
        name VARCHAR(100) NOT NULL,
        scripttoken VARCHAR(10) NOT NULL,
        accountid UUID NOT NULL REFERENCES app.accounts(id)
    );

    CREATE OR REPLACE PROCEDURE app.add_account(
        pId UUID,
        pLogin VARCHAR(100),
        pPassword BYTEA,
        pTrollId INT,
        pTrollName VARCHAR(100),
        pScriptToken VARCHAR(10))
    LANGUAGE plpgsql AS
    \$\$
    BEGIN
        INSERT INTO app.accounts(id, login, password)
        VALUES(pId, pLogin, pPassword);

        INSERT INTO app.trollaccounts(id, name, scripttoken, accountid)
        VALUES(pTrollId, pTrollName, pScriptToken, pId);
    END
    \$\$ SECURITY DEFINER;

    CREATE TYPE app.account AS (id UUID, login VARCHAR(100), password BYTEA, trollid INT, trollname VARCHAR(100), scriptToken VARCHAR(10));

    CREATE OR REPLACE FUNCTION app.get_account(pId UUID)
    RETURNS setof app.account AS
    \$\$
        SELECT a.id, a.login, a.password, t.id, t.name, t.scripttoken
        FROM app.accounts a
        INNER JOIN app.trollaccounts t on t.accountid = a.id
        WHERE a.id = pId;
    \$\$  LANGUAGE sql SECURITY DEFINER;

    CREATE OR REPLACE FUNCTION app.get_account_bylogin(pLogin VARCHAR(100))
    RETURNS setof app.account AS
    \$\$
        SELECT app.get_account(a.id)
        FROM app.accounts a
        WHERE a.login = pLogin;
    \$\$  LANGUAGE sql SECURITY DEFINER;

    CREATE OR REPLACE FUNCTION app.get_account_bytroll(pTrollId INT)
    RETURNS setof app.account AS
    \$\$
        SELECT a.id, a.login, a.password, t.id, t.name, t.scripttoken
        FROM app.accounts a
        INNER JOIN app.trollaccounts t on t.accountid = a.id
        WHERE t.id = pTrollId;
    \$\$  LANGUAGE sql SECURITY DEFINER;
EOSQL