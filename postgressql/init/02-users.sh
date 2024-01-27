#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE TABLE app.users (
        id UUID PRIMARY KEY,
        login VARCHAR(100) UNIQUE NOT NULL,
        password BYTEA NOT NULL
    );
    CREATE TABLE app.trollaccounts (
        trollid INT PRIMARY KEY,
        name VARCHAR(100) NOT NULL,
        scripttoken VARCHAR(10) NOT NULL,
        userid UUID NOT NULL REFERENCES app.users(id)
    );
    CREATE OR REPLACE PROCEDURE app.add_user(
        pId UUID,
        pLogin VARCHAR(100),
        pPassword BYTEA,
        pTrollId INT,
        pName VARCHAR(100),
        pScriptToken VARCHAR(10))
    LANGUAGE plpgsql AS
    \$\$
    BEGIN
        INSERT INTO app.users(id, login, password)
        VALUES(pId, pLogin, pPassword);

        INSERT INTO app.trollaccounts(trollid, name, scripttoken, userid)
        VALUES(pTrollId, pName, pScriptToken, pId);
    END
    \$\$ SECURITY DEFINER;
EOSQL