using Microsoft.Extensions.Configuration;

namespace CarRental.Controllers.Shared
{
    public interface IDatabaseConfiguration
    {
        string DatabaseName { get; }
        string ConnectionString { get; }
        string ProviderName { get; }
    }

    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public string DatabaseName { get; }
        public string ConnectionString { get; }
        public string ProviderName { get; }

        public DatabaseConfiguration(IConfiguration configuration)
        {
            DatabaseName = configuration["DatabaseName"] 
                          ?? throw new InvalidOperationException("DatabaseName not configured. Please ensure 'DatabaseName' is set in configuration.");

            var connectionString = configuration.GetConnectionString(DatabaseName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string '{DatabaseName}' not found in configuration. Please ensure the connection string is properly configured.");
            }
            ConnectionString = connectionString;

            // For .NET 8, we default to System.Data.SqlClient unless specified otherwise
            ProviderName = configuration[$"ConnectionStrings:{DatabaseName}:ProviderName"] ?? "System.Data.SqlClient";
        }
    }
}