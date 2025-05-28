using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace CarRental.Tests.Shared
{
    public static class DockerDatabase
    {
        public static void SetupDockerSqlServer()
        {
            try
            {
                // Create the docker-compose file if it doesn't exist
                string rootDirectory = Directory.GetCurrentDirectory();
                while (!File.Exists(Path.Combine(rootDirectory, "CarRental-Copilot.sln")) && Directory.GetParent(rootDirectory) != null)
                {
                    rootDirectory = Directory.GetParent(rootDirectory).FullName;
                }

                // Create docker-compose file
                string dockerComposeFile = Path.Combine(rootDirectory, "docker-compose.yml");
                if (!File.Exists(dockerComposeFile))
                {
                    string dockerComposeContent = @"version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: carrental-sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=CarRental#123
      - MSSQL_PID=Express
    ports:
      - ""1433:1433""
    volumes:
      - sqlserver-data:/var/opt/mssql
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ""CarRental#123"" -Q ""SELECT 1"" || exit 1
      interval: 10s
      timeout: 5s
      retries: 5

volumes:
  sqlserver-data:";
                    
                    File.WriteAllText(dockerComposeFile, dockerComposeContent);
                }

                // Start docker container
                Console.WriteLine("Starting Docker SQL Server...");
                System.Diagnostics.Process.Start("docker-compose", $"-f \"{dockerComposeFile}\" up -d");

                // Update connection string in App.config
                UpdateAppConfig();
                
                // Update environment variable for runtime configuration
                string connectionString = "Data Source=localhost,1433;Initial Catalog=CarRental;User Id=sa;******;TrustServerCertificate=True;";
                Environment.SetEnvironmentVariable("SqlServer_ConnectionString", connectionString);
                
                Console.WriteLine("Docker SQL Server setup completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up Docker SQL Server: {ex.Message}");
            }
        }

        private static void UpdateAppConfig()
        {
            try
            {
                // Update connection string in configuration
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStrings = config.ConnectionStrings;
                
                connectionStrings.ConnectionStrings["SqlServer"].ConnectionString = 
                    "Data Source=localhost,1433;Initial Catalog=CarRental;User Id=sa;******;TrustServerCertificate=True;";
                
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
                
                Console.WriteLine("Updated App.config with Docker connection string");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating App.config: {ex.Message}");
            }
        }
    }
}