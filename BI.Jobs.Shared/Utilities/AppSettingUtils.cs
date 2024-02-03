using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Jobs.Shared.Utilities
{
    public class AppSettingsUtil
    {
        private static IConfiguration _configuration;

        /// <summary>
        /// Registering the configuration depending on environment
        /// </summary>
        public static void Register(string env)
        {
            _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
        }

        /// <summary>
        /// Get AppSettings value from key
        /// </summary>
        public static T GetValue<T>(string key)
            => _configuration.GetValue<T>(key);


        public static T GetSection<T>(string key) where T : class
            => _configuration.GetSection(key).Get<T>();


        /// <summary>
        /// Get connection string
        /// </summary>
        public static string GetConnectionString(string connectionName)
            => _configuration.GetConnectionString(connectionName);
    }
}
