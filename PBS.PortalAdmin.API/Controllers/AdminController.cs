using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PBS.Core.CQRS.Query;
using PBS.PortalAdmin.API.Data.Queries;
using PBS.PortalAdmin.API.Extensions;

namespace PBS.PortalAdmin.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public AdminController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [Route("LoggedInUser")]
        public IHttpActionResult GetLoggedInUser()
        {
            return Ok(User.ToAdUserViewModel());
        }

        [Route("{CanOrCompanyName}/Users")]
        public IHttpActionResult GetPortalUsers([FromUri] GetPortalUsersQuery query)
        {

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var result = _queryDispatcher.Dispatch(query);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Route("Client/{ClientIdOrCan}/Register")]
        [HttpGet]
        public IHttpActionResult VerifyClientRegistration([FromUri] ClientRegisterationVerificationQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            
            var result = _queryDispatcher.Dispatch<bool>(query);
            
            return Ok(result);
        }


        //[Route("Submit")]
        //[HttpPost]
        //public IHttpActionResult SubmitFeedback([FromBody]SubmitFeedbackCommand submitFeedbackCommand)
        //{
        //    if (submitFeedbackCommand == null)
        //    {
        //        throw new ArgumentNullException("submitFeedbackCommand");
        //    }

        //    return Ok(submitFeedbackCommand);
        //}
     
   





    } 
}
