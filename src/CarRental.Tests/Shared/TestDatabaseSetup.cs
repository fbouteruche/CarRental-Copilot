using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRental.Tests.Shared
{
    [TestClass]
    public class TestDatabaseSetup
    {
        // This method runs once before any tests in the assembly execute
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            try
            {
                // Ensure SQL Server Docker container is running
                DockerSqlServerHelper.EnsureDockerSqlServerIsRunning();
                
                // Update connection string to use Docker container
                DockerSqlServerHelper.UpdateConnectionString();
                
                Console.WriteLine("Database setup completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during database setup: {ex.Message}");
                throw;
            }
        }

        // This method runs once after all tests in the assembly have executed
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // DockerSqlServerHelper.CleanupDockerResources();
            // Commented out to allow container reuse between test runs
        }
    }
}