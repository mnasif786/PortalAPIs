using System;
using System.Web.Http;
using System.Web.Http.Cors;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Data.Commands;

namespace PBS.Portal.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Feedback")]
    public class FeedbackController : ApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public FeedbackController( ICommandDispatcher commandDispatcher )
        {
           _commandDispatcher = commandDispatcher;
        }

        [Route("Submit")]
        [HttpPost]
        public IHttpActionResult SubmitFeedback([FromBody]SubmitFeedbackCommand submitFeedbackCommand)
        {
            if (submitFeedbackCommand == null)
            {
                throw new ArgumentNullException("submitFeedbackCommand");
            }

           _commandDispatcher.Dispatch(submitFeedbackCommand);

            return Ok();
        }

    }
}
