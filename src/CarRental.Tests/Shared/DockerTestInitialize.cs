using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRental.Tests.Shared
{
    [TestClass]
    public class DockerTestInitialize
    {
        [AssemblyInitialize]
        public static void InitializeTests(TestContext context)
        {
            try
            {
                Console.WriteLine("Initializing Docker SQL Server container...");
                
                var solutionDirectory = GetSolutionDirectory();
                var setupScriptPath = Path.Combine(solutionDirectory, "setup-docker-sqlserver.sh");
                
                if (!File.Exists(setupScriptPath))
                {
                    Console.WriteLine("Docker setup script not found. Tests may fail due to missing database connection.");
                    return;
                }
                
                // Run setup script
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = setupScriptPath,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"Error during Docker setup: {error}");
                }
                
                Console.WriteLine(output);
                
                // Update connection string settings
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connStringSettings = config.ConnectionStrings.ConnectionStrings["SqlServer"];
                
                if (connStringSettings != null)
                {
                    connStringSettings.ConnectionString = "Data Source=localhost,1433;Initial Catalog=CarRental;User Id=sa;******;TrustServerCertificate=True;";
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("connectionStrings");
                }
                
                Console.WriteLine("Docker SQL Server setup complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing Docker container: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
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