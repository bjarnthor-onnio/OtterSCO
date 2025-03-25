using Microsoft.Extensions.Configuration;

using Onnio.LoggingService.Models;

namespace Onnio.LoggingService.Extensions
{
    public static class LoggingConfigurationProvider
    {
        private const string DEFAULT_CONFIG_FILENAME = "logging.json";

        /// <summary>
        /// Loads logging configuration from a dedicated config file
        /// </summary>
        public static LoggingOptions LoadConfiguration(string configFilePath = null)
        {
            // Use default filename if not specified
            configFilePath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DEFAULT_CONFIG_FILENAME);

            var options = new LoggingOptions();

            // Check if config file exists
            if (File.Exists(configFilePath))
            {
                try
                {
                    // Create configuration from the dedicated file
                   /* var config = new ConfigurationBuilder()
                        .AddJsonFile(configFilePath, optional: false, reloadOnChange: true)
                        .Build();

                    // Bind configuration to options
                    config.GetSection("Logging").Bind(options);*/
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading logging configuration: {ex.Message}");
                    // Continue with default options on error
                }
            }

            return options;
        }
    }
}
