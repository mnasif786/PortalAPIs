using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NLog;
using PBS.Logging.Common;


namespace PBS.Logging
{
    public class LoggingService: ILoggingService
    {
        private static Logger _logger;
        private static string _applicatioName;

        public LoggingService(string applicationName)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _applicatioName = applicationName;
        }

        public void LogError(Exception exception, string message)
        {
            _logger.Log(LogLevel.Error, exception, message);
        }

        public void LogError(Exception exception, params object[] args)
        {
            var logEventInfo = CreateLogEventInfo(LogLevel.Error, exception, args);
            _logger.Log(logEventInfo);
        }

        
        private static LogEventInfo CreateLogEventInfo(LogLevel logLevel, Exception exception, object[] args)
        {
            var logEventInfo = new LogEventInfo(logLevel, _logger.Name, null, exception.Message, args, exception);

            logEventInfo.Properties["ApplicationName"] = _applicatioName;

            foreach (var arg in args.OfType<HttpRequestMessage>())
            {
                logEventInfo.Properties["Request"] = string.Format("URL: {0},  Method: {1}", arg.RequestUri, arg.Method);
            }

           
            return logEventInfo;
        }
    }

   
}
