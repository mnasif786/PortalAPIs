using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using PBS.Logging;

namespace PBS.Advice.API
{
    public class AdviceExceptionLogger: ExceptionLogger
    {
        private readonly ILoggingService _loggingService;

        public AdviceExceptionLogger(ILoggingService loggingService)
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