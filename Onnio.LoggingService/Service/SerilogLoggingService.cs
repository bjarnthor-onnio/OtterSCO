using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.LoggingService.Interface;
using Serilog;

namespace Onnio.LoggingService.Service
{
    public class SerilogLoggingService : ILoggingService
    {
        private readonly ILogger _logger;

        public string LoggerName { get; }

        public SerilogLoggingService(ILogger logger, string loggerName)
        {
            _logger = logger?.ForContext("LoggerName", loggerName) ??
                throw new ArgumentNullException(nameof(logger));
            LoggerName = loggerName;
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warning(message);
        }

        public void LogError(string message, Exception exception = null)
        {
            if (exception != null)
                _logger.Error(exception, message);
            else
                _logger.Error(message);
        }

        public void LogError(Exception exception)
        {
            _logger.Error(exception, exception.Message);
        }

        public void LogCritical(string message, Exception exception = null)
        {
            if (exception != null)
                _logger.Fatal(exception, message);
            else
                _logger.Fatal(message);
        }
    }
}
