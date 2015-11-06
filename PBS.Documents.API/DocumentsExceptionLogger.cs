using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using PBS.Logging;

namespace PBS.Documents.API
{
    public class DocumentsExceptionLogger : ExceptionLogger
    {
        private readonly ILoggingService _loggingService;

        public DocumentsExceptionLogger(ILoggingService loggingService)
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