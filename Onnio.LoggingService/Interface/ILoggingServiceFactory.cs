using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onnio.LoggingService.Interface
{
    using System;
    public interface ILoggingServiceFactory
    {
        ILoggingService GetLogger(string loggerName);
        ILoggingService GetLogger<T>();
    }

}
