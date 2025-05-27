#!/bin/bash

# Wait for SQL Server to be ready
sleep 10

# Run the initialization script
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'CarRental#123' -i /scripts/init-db.sql

# Add execution permissions to this file with:
# chmod +x ./sql-scripts/run-init-script.sh