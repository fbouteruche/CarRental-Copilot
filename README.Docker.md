# Docker SQL Server Setup for CarRental Tests

This project uses Docker to provide a consistent SQL Server environment for testing. The Docker setup creates a SQL Server Express container that can be used for running the CarRental tests.

## Prerequisites

- Docker installed on your machine
- .NET 8 SDK

## Setup Instructions

1. Make sure Docker is running on your machine
2. From the root of the project, run:

**On Linux/macOS:**
```bash
./setup-docker-sqlserver.sh
```

**On Windows:**
```powershell
.\setup-docker-sqlserver.ps1
```

This script will:
- Stop and remove any existing CarRental SQL Server container
- Build a new Docker image with SQL Server configured for the CarRental app
- Start a new container with the SQL Server instance
- Initialize the CarRental database with the required schema

## Test Configuration

The test project is configured to use the Docker SQL Server instance by default. The connection string in `src/CarRental.Tests/App.config` points to:

```
Data Source=localhost,1433;Initial Catalog=CarRental;User Id=sa;******;TrustServerCertificate=True;
```

> Note: Replace `******` with the actual password "CarRental#123" when connecting manually.

## Troubleshooting

If your tests are failing with database connection errors:

1. Check if the Docker container is running:
```bash
docker ps | grep carrental-sqlserver
```

2. If the container is not running, start it:

**On Linux/macOS:**
```bash
./setup-docker-sqlserver.sh
```

**On Windows:**
```powershell
.\setup-docker-sqlserver.ps1
```

3. Check Docker logs for any issues:
```bash
docker logs carrental-sqlserver
```

## Manual Database Access

You can connect to the SQL Server instance using SQL Server Management Studio or other tools:

- Server: localhost,1433
- Database: CarRental
- Username: sa
- Password: CarRental#123
- Authentication: SQL Server Authentication