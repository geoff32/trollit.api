#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
	CREATE SCHEMA IF NOT EXISTS app;

    CREATE USER $POSTGRES_WEBAPI_USER with encrypted password '$POSTGRES_WEBAPI_PASSWORD';
    GRANT CONNECT, CREATE ON DATABASE $POSTGRES_DB TO $POSTGRES_WEBAPI_USER;

    GRANT ALL PRIVILEGES ON SCHEMA app TO $POSTGRES_WEBAPI_USER;
EOSQL