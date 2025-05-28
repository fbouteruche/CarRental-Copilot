# PowerShell script to set up Docker SQL Server for CarRental tests

# Stop and remove any existing container
Write-Host "Stopping any existing containers..."
docker stop carrental-sqlserver 2>$null
docker rm carrental-sqlserver 2>$null

# Build the SQL Server Docker image
Write-Host "Building SQL Server Docker image..."
docker build -t carrental-sqlserver -f Dockerfile.sqlserver .

# Run the SQL Server container
Write-Host "Starting SQL Server container..."
docker run -d --name carrental-sqlserver -p 1433:1433 carrental-sqlserver

# Wait for SQL Server to be ready
Write-Host "Waiting for SQL Server to start..."
Start-Sleep -Seconds 45

Write-Host "SQL Server container is now running with the CarRental database initialized."