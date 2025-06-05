using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using CarRental.Controllers.Shared;

namespace CarRental.Controllers.Tests.Shared
{
    [TestClass]
    public class TestDatabaseSetup
    {
        private static bool _dockerInitializationAttempted = false;
        private static bool _dockerInitializationSucceeded = false;

        // This method runs once before any tests in the assembly execute
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            _dockerInitializationAttempted = true;
            
            try
            {
                // Initialize configuration
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                // Initialize database configuration
                var dbConfig = new DatabaseConfiguration(configuration);
                Db.Initialize(dbConfig);

                // Check if Docker is available
                if (!IsDockerAvailable())
                {
                    Console.WriteLine("Docker is not available. Skipping Docker SQL Server setup.");
                    Console.WriteLine("Database-dependent tests will be skipped or may fail.");
                    return;
                }

                // Ensure SQL Server Docker container is running
                DockerSqlServerHelper.EnsureDockerSqlServerIsRunning();
                
                // Update connection string to use Docker container
                DockerSqlServerHelper.UpdateConnectionString(configuration);
                
                _dockerInitializationSucceeded = true;
                Console.WriteLine("Database setup completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Docker database setup failed: {ex.Message}");
                Console.WriteLine("Database-dependent tests will be skipped or may fail.");
                // Don't throw the exception to allow domain tests to run
            }
        }

        // This method runs once after all tests in the assembly have executed
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (_dockerInitializationSucceeded)
            {
                // DockerSqlServerHelper.CleanupDockerResources();
                // Commented out to allow container reuse between test runs
            }
        }

        public static bool IsDockerDatabaseAvailable()
        {
            return _dockerInitializationAttempted && _dockerInitializationSucceeded;
        }

        private static bool IsDockerAvailable()
        {
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "docker",
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}