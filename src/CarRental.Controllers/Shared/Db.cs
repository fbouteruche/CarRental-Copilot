using System;
using System.Collections.Generic;
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
        private static IDatabaseConfiguration? _configuration;
        private static DbProviderFactory? _providerFactory;
        private static bool _initialized = false;
        private static readonly object _lockObject = new object();

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

        public static void Initialize(IDatabaseConfiguration configuration)
        {
            lock (_lockObject)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                
                try
                {
                    _providerFactory = DbProviderFactories.GetFactory(_configuration.ProviderName);
                    _initialized = true;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Failed to create database provider factory for provider '{_configuration.ProviderName}'. Please ensure the provider is properly installed and configured.", ex);
                }
            }
        }

        private static void EnsureInitialized()
        {
            if (!_initialized || _configuration == null)
            {
                throw new InvalidOperationException(
                    "Database configuration not initialized. Please call Db.Initialize() with a valid configuration before using the database.");
            }
        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            EnsureInitialized();
            using (IDbConnection connection = _providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = _configuration!.ConnectionString;

                using (IDbCommand command = _providerFactory!.CreateCommand()!)
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
            using (IDbConnection connection = _providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = _configuration!.ConnectionString;

                using (IDbCommand command = _providerFactory!.CreateCommand()!)
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
            using (IDbConnection connection = _providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = _configuration!.ConnectionString;

                using (IDbCommand command = _providerFactory!.CreateCommand()!)
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
            using (IDbConnection connection = _providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = _configuration!.ConnectionString;

                using (IDbCommand command = _providerFactory!.CreateCommand()!)
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
            using (IDbConnection connection = _providerFactory!.CreateConnection()!)
            {
                connection.ConnectionString = _configuration!.ConnectionString;

                using (IDbCommand command = _providerFactory!.CreateCommand()!)
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
            switch (_configuration!.ProviderName)
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
