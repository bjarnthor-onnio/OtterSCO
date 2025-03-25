using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Onnio.LoggingService.Interface;
using Onnio.LoggingService.Service;
using Serilog.Core;

namespace Onnio.LoggingService.Factory
{
    public class SerilogLoggingServiceFactory : ILoggingServiceFactory
    {
        private readonly Logger _rootLogger;
        private readonly ConcurrentDictionary<string, ILoggingService> _loggers = new();

        public SerilogLoggingServiceFactory(Logger rootLogger)
        {
            _rootLogger = rootLogger ?? throw new ArgumentNullException(nameof(rootLogger));
        }

        public ILoggingService GetLogger(string loggerName)
        {
            if (string.IsNullOrWhiteSpace(loggerName))
                throw new ArgumentException("Logger name cannot be null or empty", nameof(loggerName));

            return _loggers.GetOrAdd(loggerName, name => new SerilogLoggingService(_rootLogger, name));
        }

        public ILoggingService GetLogger<T>()
        {
            return GetLogger(typeof(T).FullName);
        }
    }
}
