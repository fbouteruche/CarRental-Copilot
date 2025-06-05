using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using CarRental.Controllers.Shared;

namespace CarRental.Controllers.Tests.Shared
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
        /// Updates the database configuration with Docker SQL Server details.
        /// </summary>
        public static void UpdateConnectionString(IConfiguration configuration)
        {
            var dockerConnectionString = "Data Source=127.0.0.1,1433;Initial Catalog=CarRental;User Id=sa;Password=CarRental#123;TrustServerCertificate=True;";
            
            try
            {
                // For the modern configuration approach, we need to reinitialize the Db class
                // with the updated connection string for Docker
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string>("ConnectionStrings:SqlServer", dockerConnectionString)
                    });

                var updatedConfig = builder.Build();
                var dbConfig = new DatabaseConfiguration(updatedConfig);
                Db.Initialize(dbConfig);
                
                Console.WriteLine("Updated database configuration with Docker connection string");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating configuration: {ex.Message}");
            }
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