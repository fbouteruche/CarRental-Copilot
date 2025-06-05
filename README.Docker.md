# Docker SQL Server Setup for CarRental Tests

This project uses Docker to provide a consistent SQL Server environment for testing. The Docker setup creates a SQL Server Express container that can be used for running the CarRental tests.

## Prerequisites

- Docker installed on your machine
- .NET 8 SDK

## Setup Instructions

1. Make sure Docker is running on your machine
2. From the root of the project, run:

```bash
docker compose up -d
```

This command will:
- Build a new Docker image with SQL Server configured for the CarRental app
- Start a new container with the SQL Server instance
- Initialize the CarRental database with the required schema
- Set up health checks to ensure the database is ready

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
```bash
docker compose up -d
```

3. Check Docker logs for any issues:
```bash
docker logs carrental-sqlserver
```

4. If you see errors, try rebuilding the image:
```bash
docker compose down
docker compose build --no-cache
docker compose up -d
```

5. View the container's detailed logs:
```bash
docker exec -it carrental-sqlserver ls -la /opt/mssql-tools/bin
docker exec -it carrental-sqlserver bash -c 'echo $PATH'
```

## Manual Database Access

You can connect to the SQL Server instance using SQL Server Management Studio or other tools:

- Server: localhost,1433
- Database: CarRental
- Username: sa
- Password: CarRental#123
- Authentication: SQL Server Authentication