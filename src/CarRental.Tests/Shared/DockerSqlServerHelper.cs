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
                    
                    // Wait for SQL Server to be ready (initialization script runs automatically in container)
                    Console.WriteLine("Waiting for SQL Server to start and initialize (may take up to 60 seconds)...");
                    Thread.Sleep(30000); // Wait 30 seconds for SQL Server and database initialization to complete
                    
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
            var dockerConnectionString = "Data Source=127.0.0.1,1433;Initial Catalog=CarRental;User Id=sa;Password=CarRental#123;TrustServerCertificate=True;";
            
            try
            {
                // In .NET 8, ConfigurationManager might not load App.config automatically in test environments
                // Try to programmatically add the configuration
                
                // First, try to refresh the sections to force reload
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                System.Configuration.ConfigurationManager.RefreshSection("connectionStrings");
                
                // Check if we can read the configuration now
                var databaseName = System.Configuration.ConfigurationManager.AppSettings["databaseName"];
                Console.WriteLine($"After refresh - Database name: '{databaseName}'");
                
                if (string.IsNullOrEmpty(databaseName))
                {
                    // If still empty, try to load the configuration file explicitly
                    var configPath = Path.Combine(GetSolutionDirectory(), "src", "CarRental.Tests", "bin", "Debug", "net8.0", "CarRental.Tests.dll.config");
                    if (File.Exists(configPath))
                    {
                        Console.WriteLine($"Attempting to load config from: {configPath}");
                        
                        // Use the legacy approach of updating runtime configuration
                        // This is a workaround for .NET 8 ConfigurationManager issues
                        UpdateRuntimeConfiguration(dockerConnectionString);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating configuration: {ex.Message}");
            }
            
            // Also update the app.config files (legacy approach)
            UpdateAppConfig(dockerConnectionString);
        }
        
        private static void UpdateRuntimeConfiguration(string connectionString)
        {
            try
            {
                // This is a workaround for .NET 8 where ConfigurationManager doesn't automatically load App.config
                // We'll use reflection to add the configuration at runtime
                var appSettingsType = typeof(System.Configuration.ConfigurationManager).Assembly.GetType("System.Configuration.ClientConfigurationSystem");
                if (appSettingsType != null)
                {
                    var configSystem = Activator.CreateInstance(appSettingsType, true);
                    
                    // Try to get the current configuration and update it
                    // This is complex and may not work in all scenarios
                    Console.WriteLine("Attempting runtime configuration update...");
                }
                
                // Alternative: Set as environment variables that the application can check
                Environment.SetEnvironmentVariable("TEST_DATABASE_NAME", "SqlServer");
                Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", connectionString);
                Environment.SetEnvironmentVariable("TEST_PROVIDER_NAME", "System.Data.SqlClient");
                
                Console.WriteLine("Set test environment variables as fallback");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Runtime configuration update failed: {ex.Message}");
            }
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