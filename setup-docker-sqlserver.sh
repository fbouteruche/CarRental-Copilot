#!/bin/bash

# Stop and remove any existing container
echo "Stopping any existing containers..."
docker stop carrental-sqlserver 2>/dev/null
docker rm carrental-sqlserver 2>/dev/null

# Build the SQL Server Docker image
echo "Building SQL Server Docker image..."
docker build -t carrental-sqlserver -f Dockerfile.sqlserver .

# Run the SQL Server container
echo "Starting SQL Server container..."
docker run -d --name carrental-sqlserver -p 1433:1433 carrental-sqlserver

# Wait for SQL Server to be ready
echo "Waiting for SQL Server to start..."
sleep 15

echo "SQL Server container is now running with the CarRental database initialized."