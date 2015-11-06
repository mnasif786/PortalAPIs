using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using PBS.Logging;
using PBS.Logging.Common;

namespace PBS.Portal.API
{
    public class GlobalExceptionLogger: ExceptionLogger
    {
        private readonly ILoggingService _loggingService;
        
        public GlobalExceptionLogger(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }
        
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            _loggingService.LogError(context.Exception, context.Request);
            return base.LogAsync(context, cancellationToken);
        }
    }
}