using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Controllers.Shared
{
    public delegate T ConverterDelegate<T>(IDataReader reader);

    public static class Db
    {
        private static readonly string databaseName;
        private static readonly string connectionString;
        private static readonly string providerName;
        private static readonly DbProviderFactory providerFactory;

        static Db()
        {
            // Register SQL Client provider if not already registered (needed for .NET Core)
            try
            {
                if (!DbProviderFactories.GetProviderInvariantNames().Contains("System.Data.SqlClient"))
                {
                    DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
                }
            }
            catch (Exception)
            {
                // Ignore registration errors as it might already be registered
            }

            // First, try to get the configuration from the normal App.config
            var databaseNameConfig = ConfigurationManager.AppSettings["databaseName"];
            
            // If the regular config is not available, check for Docker test environment
            if (string.IsNullOrEmpty(databaseNameConfig))
            {
                // Check if we're in a test environment with Docker
                if (IsDockerTestEnvironment())
                {
                    // Use Docker defaults
                    databaseName = "SqlServer";
                    connectionString = "Data Source=localhost,1433;Initial Catalog=CarRental;User Id=sa;Password=CarRental#123;TrustServerCertificate=True;";
                    providerName = "System.Data.SqlClient";
                    
                    try
                    {
                        providerFactory = DbProviderFactories.GetFactory(providerName);
                        return; // Successfully initialized with Docker defaults
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Failed to create database provider factory for provider '{providerName}' in Docker test environment. Please ensure the provider is properly installed and configured.", ex);
                    }
                }
                
                // If not in Docker environment, throw the original error
                throw new InvalidOperationException(
                    "Database name not configured. Please ensure 'databaseName' is set in appSettings in your App.config file.");
            }
            
            databaseName = databaseNameConfig;

            var connectionStringConfig = ConfigurationManager.ConnectionStrings[databaseName];
            if (connectionStringConfig == null)
            {
                throw new InvalidOperationException(
                    $"Connection string '{databaseName}' not found in configuration. Please ensure the connection string is properly configured in your App.config file.");
            }

            connectionString = connectionStringConfig.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{databaseName}' is empty. Please ensure the connection string value is properly set in your App.config file.");
            }

            providerName = connectionStringConfig.ProviderName;
            if (string.IsNullOrEmpty(providerName))
            {
                throw new InvalidOperationException(
                    $"Provider name for connection string '{databaseName}' is not specified. Please ensure the providerName attribute is set in your App.config file.");
            }

            try
            {
                providerFactory = DbProviderFactories.GetFactory(providerName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Failed to create database provider factory for provider '{providerName}'. Please ensure the provider is properly installed and configured.", ex);
            }
        }
        
        private static bool IsDockerTestEnvironment()
        {
            // Check if we're running in a test environment
            var isTestEnvironment = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.Contains("Test") ||
                                   Environment.CommandLine.Contains("testhost") ||
                                   Environment.CommandLine.Contains("dotnet test");
            
            if (!isTestEnvironment)
                return false;
                
            // Check if Docker is available and CarRental SQL Server container is running
            try
            {
                var process = new System.Diagnostics.Process
                {
                    StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "docker",
                        Arguments = "ps --filter name=carrental-sqlserver --format {{.Names}}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                process.WaitForExit(5000); // 5 second timeout
                
                if (process.ExitCode == 0)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    return output?.Contains("carrental-sqlserver") == true;
                }
            }
            catch
            {
                // If Docker is not available or any error occurs, return false
            }
            
            return false;
        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = providerFactory.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory.CreateCommand()!)
                {
                    command.CommandText = sql.AppendSelectIdentity();
                    command.Connection = connection;
                    command.SetParameters(parameters);

                    connection.Open();

                    int id = Convert.ToInt32(command.ExecuteScalar());

                    return id;
                }
            }
        }

        public static void Update(string sql, Dictionary<string, object>? parameters = null)
        {
            using (IDbConnection connection = providerFactory.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory.CreateCommand()!)
                {
                    command.CommandText = sql;

                    command.Connection = connection;

                    command.SetParameters(parameters);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Delete(string sql, Dictionary<string, object> parameters)
        {
            Update(sql, parameters);
        }

        public static List<T> GetAll<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object>? parameters = null)
        {
            using (IDbConnection connection = providerFactory.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory.CreateCommand()!)
                {
                    command.CommandText = sql;

                    command.Connection = connection;

                    command.SetParameters(parameters);

                    connection.Open();

                    var list = new List<T>();

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var obj = convert(reader);
                            list.Add(obj);
                        }

                        return list;
                    }
                }
            }
        }

        public static T? Get<T>(string sql, ConverterDelegate<T> convert, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = providerFactory.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory.CreateCommand()!)
                {
                    command.CommandText = sql;

                    command.Connection = connection;

                    command.SetParameters(parameters);

                    connection.Open();

                    T? t = default;

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            t = convert(reader);

                        return t;
                    }
                }
            }
        }

        public static bool Exists(string sql, Dictionary<string, object> parameters)
        {
            using (IDbConnection connection = providerFactory.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory.CreateCommand()!)
                {
                    command.CommandText = sql;

                    command.Connection = connection;

                    command.SetParameters(parameters);

                    connection.Open();

                    int numberRows = Convert.ToInt32(command.ExecuteScalar());

                    return numberRows > 0;
                }
            }
        }

        private static void SetParameters(this IDbCommand command, Dictionary<string, object>? parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return;

            foreach (var parameter in parameters)
            {
                string name = parameter.Key;

                object value = parameter.Value.IsNullOrEmpty() ? DBNull.Value : parameter.Value;

                IDataParameter dbParameter = command.CreateParameter();

                dbParameter.ParameterName = name;
                dbParameter.Value = value;

                command.Parameters.Add(dbParameter);
            }
        }

        private static string AppendSelectIdentity(this string sql)
        {
            switch (providerName)
            {
                case "System.Data.SqlClient": return sql + ";SELECT SCOPE_IDENTITY()";

                case "System.Data.SQLite": return sql + ";SELECT LAST_INSERT_ROWID()";

                default: return sql;
            }
        }

        public static bool IsNullOrEmpty(this object? value)
        {
            return (value is string && string.IsNullOrEmpty((string)value)) ||
                    value == null;
        }

    }
}
