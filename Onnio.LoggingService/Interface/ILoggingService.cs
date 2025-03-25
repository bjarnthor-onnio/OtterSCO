using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.LoggingService.Interface
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception = null);
        void LogError(Exception exception);
        void LogCritical(string message, Exception exception = null);
        string LoggerName { get; }
    }
}
