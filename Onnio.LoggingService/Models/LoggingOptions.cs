using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog;

namespace Onnio.LoggingService.Models
{
    public class LoggingOptions
    {
        /// <summary>
        /// The directory where log files will be stored
        /// </summary>
        public string LogDirectory { get; set; } = Path.Combine("C:\\ProgramData\\LS Retail\\LS Self-Checkout Connector", "Logs");

        /// <summary>
        /// The rolling interval for log files
        /// </summary>
        public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;

        /// <summary>
        /// Minimum log level to be recorded
        /// </summary>
        public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Debug;

        /// <summary>
        /// Whether to enable console logging
        /// </summary>
        public bool EnableConsoleLogging { get; set; } = true;
    }
}
