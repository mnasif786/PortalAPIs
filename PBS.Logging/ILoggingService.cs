using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using PBS.Logging.Common;

namespace PBS.Logging
{
    public interface ILoggingService
    {
        void LogError(Exception exception, string message);
        void LogError(Exception exception, params object[] arg);
       
      
    }
}
