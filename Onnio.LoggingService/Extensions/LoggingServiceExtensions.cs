using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Core;
using Serilog.Sinks.File;
using Onnio.LoggingService.Interface;
using Onnio.LoggingService.Models;
using Onnio.LoggingService.Factory;

namespace Onnio.LoggingService.Extensions
{
    public static class LoggingServiceExtensions
    {
        /// <summary>
        /// Adds the Serilog logging service to the service collection using a dedicated config file
        /// </summary>
        public static IServiceCollection AddSerilogServices(
        this IServiceCollection services,
        string configFilePath = null,
        Action<LoggerConfiguration> configureLogger = null)
        {
            // Load options from the dedicated config file
            var options = LoggingConfigurationProvider.LoadConfiguration(configFilePath);

            return AddSerilogServicesWithOptions(services, options, configureLogger);
        }

        /// <summary>
        /// Adds the Serilog logging service to the service collection with custom options
        /// </summary>
        public static IServiceCollection AddSerilogServices(
            this IServiceCollection services,
            LoggingOptions options,
            Action<LoggerConfiguration> configureLogger = null)
        {
            return AddSerilogServicesWithOptions(services, options, configureLogger);
        }

        /// <summary>
        /// Adds the Serilog logging service to the service collection with options builder
        /// </summary>
        public static IServiceCollection AddSerilogServices(
            this IServiceCollection services,
            Action<LoggingOptions> configureOptions,
            Action<LoggerConfiguration> configureLogger = null)
        {
            // Create and configure options
            var options = new LoggingOptions();
            configureOptions?.Invoke(options);

            return AddSerilogServicesWithOptions(services, options, configureLogger);
        }

        /// <summary>
        /// Internal method to add Serilog services with the specified options
        /// </summary>
        private static IServiceCollection AddSerilogServicesWithOptions(
            IServiceCollection services,
            LoggingOptions options,
            Action<LoggerConfiguration> configureLogger = null)
        {
            // Ensure the log directory exists
            if (!Directory.Exists(options.LogDirectory))
                Directory.CreateDirectory(options.LogDirectory);

            // Create the logger configuration
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Is(options.MinimumLevel)
                // Write to separate log files based on log level
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                    .WriteTo.File(
                        Path.Combine(options.LogDirectory, "debug-.log"),
                        rollingInterval: options.RollingInterval,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{LoggerName}] {Message:lj}{NewLine}{Exception}"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        Path.Combine(options.LogDirectory, "info-.log"),
                        rollingInterval: options.RollingInterval,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{LoggerName}] {Message:lj}{NewLine}{Exception}"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level >= LogEventLevel.Warning)
                    .WriteTo.File(
                        Path.Combine(options.LogDirectory, "error-.log"),
                        rollingInterval: options.RollingInterval,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{LoggerName}] {Message:lj}{NewLine}{Exception}"))
                // For component-specific logs
                .WriteTo.Map("LoggerName", "default", (loggerName, wt) => wt
                    .File(
                        Path.Combine(options.LogDirectory, $"{loggerName}-.log"),
                        rollingInterval: options.RollingInterval,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

            // Add console logging if enabled
            if (options.EnableConsoleLogging)
            {
                loggerConfig.WriteTo.Console(
                    outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] [{LoggerName}] {Message:lj}{NewLine}{Exception}");
            }

            // Apply custom configuration if provided
            configureLogger?.Invoke(loggerConfig);

            // Create the root logger
            Logger rootLogger = loggerConfig.CreateLogger();

            // Register services
            services.AddSingleton(rootLogger);
            services.AddSingleton<ILoggingServiceFactory, SerilogLoggingServiceFactory>();

            // Register a default logger for simple injection cases
            services.AddSingleton<ILoggingService>(provider =>
                provider.GetRequiredService<ILoggingServiceFactory>().GetLogger("Default"));

            return services;
        }
    }
}
