using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace CarRental.BlazorApp.Services
{
    public class ConfigurationAdapter : ConfigurationManager
    {
        private readonly IConfiguration _configuration;
        private static ConfigurationAdapter? _instance;

        private ConfigurationAdapter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void Initialize(IConfiguration configuration)
        {
            _instance = new ConfigurationAdapter(configuration);
        }

        public new static ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("ConfigurationAdapter not initialized. Call Initialize first.");

                var connectionStrings = new ConnectionStringSettingsCollection();
                
                var section = _instance._configuration.GetSection("ConnectionStrings");
                if (section.Exists())
                {
                    foreach (var child in section.GetChildren())
                    {
                        var providerName = "System.Data.SqlClient"; // default provider
                        connectionStrings.Add(new ConnectionStringSettings(child.Key, child.Value, providerName));
                    }
                }
                
                return connectionStrings;
            }
        }

        public new static NameValueCollection AppSettings
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("ConfigurationAdapter not initialized. Call Initialize first.");

                var appSettings = new NameValueCollection();
                
                var section = _instance._configuration.GetSection("AppSettings");
                if (section.Exists())
                {
                    foreach (var child in section.GetChildren())
                    {
                        appSettings.Add(child.Key, child.Value);
                    }
                }
                
                return appSettings;
            }
        }
    }
}