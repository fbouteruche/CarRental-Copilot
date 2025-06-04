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
        private static string? databaseName;
        private static string? connectionString;
        private static string? providerName;
        private static DbProviderFactory? providerFactory;
        private static bool initialized = false;
        private static readonly object lockObject = new object();

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
        }

        private static void EnsureInitialized()
        {
            if (initialized)
                return;

            lock (lockObject)
            {
                if (initialized)
                    return;

                var databaseNameConfig = ConfigurationManager.AppSettings["databaseName"];
                
                // Fallback to environment variables for test scenarios (needed for .NET 8)
                if (string.IsNullOrEmpty(databaseNameConfig))
                {
                    databaseNameConfig = Environment.GetEnvironmentVariable("TEST_DATABASE_NAME");
                }
                
                if (string.IsNullOrEmpty(databaseNameConfig))
                {
                    throw new InvalidOperationException(
                        "Database name not configured. Please ensure 'databaseName' is set in appSettings in your App.config file.");
                }
                databaseName = databaseNameConfig;

                var connectionStringConfig = ConfigurationManager.ConnectionStrings[databaseName];
                string? connectionStringValue = connectionStringConfig?.ConnectionString;
                string? providerNameValue = connectionStringConfig?.ProviderName;
                
                // Fallback to environment variables for test scenarios (needed for .NET 8)
                if (string.IsNullOrEmpty(connectionStringValue))
                {
                    connectionStringValue = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING");
                }
                
                if (string.IsNullOrEmpty(providerNameValue))
                {
                    providerNameValue = Environment.GetEnvironmentVariable("TEST_PROVIDER_NAME");
                }

                if (connectionStringConfig == null && string.IsNullOrEmpty(connectionStringValue))
                {
                    throw new InvalidOperationException(
                        $"Connection string '{databaseName}' not found in configuration. Please ensure the connection string is properly configured in your App.config file.");
                }

                connectionString = connectionStringValue;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException(
                        $"Connection string '{databaseName}' is empty. Please ensure the connection string value is properly set in your App.config file.");
                }

                providerName = providerNameValue;
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

                initialized = true;
            }
        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            EnsureInitialized();
            using (IDbConnection connection = providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory!.CreateCommand()!)
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
            EnsureInitialized();
            using (IDbConnection connection = providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory!.CreateCommand()!)
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
            EnsureInitialized();
            using (IDbConnection connection = providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory!.CreateCommand()!)
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
            EnsureInitialized();
            using (IDbConnection connection = providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory!.CreateCommand()!)
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
            EnsureInitialized();
            using (IDbConnection connection = providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = connectionString;

                using (IDbCommand command = providerFactory!.CreateCommand()!)
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
            EnsureInitialized();
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
