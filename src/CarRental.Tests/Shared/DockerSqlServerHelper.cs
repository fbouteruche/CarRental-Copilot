using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CarRental.Tests.Shared
{
    public static class DockerSqlServerHelper
    {
        private static bool _containerStarted = false;
        private static readonly string DockerComposeFilePath = Path.Combine(GetSolutionDirectory(), "docker-compose.yml");
        private static readonly string SqlScriptPath = Path.Combine(GetSolutionDirectory(), "sql-scripts");
        private static readonly object _lock = new object();

        /// <summary>
        /// Starts the SQL Server Docker container if not already running.
        /// </summary>
        public static void EnsureDockerSqlServerIsRunning()
        {
            lock (_lock)
            {
                if (_containerStarted)
                {
                    return;
                }

                // Check if the container is already running
                if (IsContainerRunning("carrental-sqlserver"))
                {
                    Console.WriteLine("Docker SQL Server container is already running.");
                    _containerStarted = true;
                    return;
                }

                try
                {
                    Console.WriteLine("Starting Docker SQL Server container...");
                    
                    // Start the container using docker-compose
                    RunProcess("docker-compose", $"-f \"{DockerComposeFilePath}\" up -d");
                    
                    // Wait for SQL Server to be ready
                    Console.WriteLine("Waiting for SQL Server to start (may take up to 60 seconds)...");
                    Thread.Sleep(30000); // Wait 30 seconds for SQL Server to initialize
                    
                    // Run the initialization script inside the container
                    RunProcess("docker", "exec carrental-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P \"CarRental#123\" -C -i /usr/src/app/init-db.sql");
                    
                    Console.WriteLine("Docker SQL Server container is now running and initialized.");
                    _containerStarted = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error starting Docker SQL Server: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates the app.config connection string with Docker SQL Server details.
        /// </summary>
        public static void UpdateConnectionString()
        {
            var dockerConnectionString = "Data Source=localhost,1433;Initial Catalog=CarRental;User Id=sa;Password=CarRental#123;TrustServerCertificate=True;";
            Environment.SetEnvironmentVariable("CarRental_ConnectionString", dockerConnectionString);
            
            // Also update the app.config
            UpdateAppConfig(dockerConnectionString);
        }
        
        /// <summary>
        /// Updates the app.config file with Docker SQL Server connection string.
        /// </summary>
        private static void UpdateAppConfig(string connectionString)
        {
            try
            {
                // Update the source App.config
                var configFile = Path.Combine(GetSolutionDirectory(), "src", "CarRental.Tests", "App.config");
                UpdateConfigFile(configFile, connectionString);
                
                // Also update the runtime App.config in the bin directory if it exists
                var runtimeConfigFile = Path.Combine(GetSolutionDirectory(), "src", "CarRental.Tests", "bin", "Debug", "net8.0", "App.config");
                if (File.Exists(runtimeConfigFile))
                {
                    UpdateConfigFile(runtimeConfigFile, connectionString);
                }
                
                // Update the dll.config file as well
                var dllConfigFile = Path.Combine(GetSolutionDirectory(), "src", "CarRental.Tests", "bin", "Debug", "net8.0", "CarRental.Tests.dll.config");
                if (File.Exists(dllConfigFile))
                {
                    UpdateConfigFile(dllConfigFile, connectionString);
                }
                
                Console.WriteLine("Updated App.config with Docker connection string");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating App.config: {ex.Message}");
            }
        }
        
        private static void UpdateConfigFile(string configFile, string connectionString)
        {
            string configContent = File.ReadAllText(configFile);
            
            // Replace the connection string in config
            var updatedContent = System.Text.RegularExpressions.Regex.Replace(
                configContent,
                @"<add name=""SqlServer""[^>]*connectionString=""[^""]*""",
                $"<add name=\"SqlServer\" providerName=\"System.Data.SqlClient\" connectionString=\"{connectionString}\"");
            
            File.WriteAllText(configFile, updatedContent);
        }

        /// <summary>
        /// Cleans up Docker resources when tests are done.
        /// </summary>
        public static void CleanupDockerResources()
        {
            // This method could be called when tests are complete to stop and remove the container
            // For continuous integration this might be left out to allow reuse of the container between test runs
        }

        private static bool IsContainerRunning(string containerName)
        {
            var process = RunProcess("docker", $"ps --filter name={containerName} --format {{{{.Names}}}}", waitForExit: true);
            return process.StandardOutput.ReadToEnd().Contains(containerName);
        }

        private static Process RunProcess(string command, string arguments, bool waitForExit = true)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            
            if (waitForExit)
            {
                process.WaitForExit();
                var error = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"Process error: {error}");
                }
            }

            return process;
        }

        private static string GetSolutionDirectory()
        {
            var directory = Directory.GetCurrentDirectory();
            while (directory != null && !File.Exists(Path.Combine(directory, "CarRental-Copilot.sln")))
            {
                directory = Directory.GetParent(directory)?.FullName;
            }
            
            return directory ?? throw new DirectoryNotFoundException("Solution directory not found");
        }
    }
}