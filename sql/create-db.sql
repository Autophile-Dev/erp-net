-- Run as postgres superuser. Change password before use.
CREATE ROLE erp_app LOGIN PASSWORD 'CHANGE_TO_A_STRONG_UNIQUE_PASSWORD';
-- Run the next statement separately in pgAdmin if needed.
CREATE DATABASE erp_local OWNER erp_app;
