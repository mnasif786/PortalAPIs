using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Http;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Models;
using StructureMap;


namespace PBS.Portal.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

       
        public UserController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
           _queryDispatcher = queryDispatcher;           
           _commandDispatcher = commandDispatcher;
        }
        
        [Route("Profile")]
        public IHttpActionResult GetUserProfile([FromUri]GetUserProfileByUserNameQuery getUserProfileByUserNameQuery)
        {
            if (getUserProfileByUserNameQuery == null)
            {
                throw new ArgumentNullException("getUserProfileByUserNameQuery");
            }

            var result = _queryDispatcher.Dispatch(getUserProfileByUserNameQuery);          

            return Ok(result);
        }

        [Route("SendRegistrationEmail")]
        [HttpPost]
        public IHttpActionResult SendRegistrationEMail(SendRegistrationEmailCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("SendRegistrationEmailCommand");
            }

            _commandDispatcher.Dispatch(cmd);

            return Ok();
        }


        [Route("SendRegistrationEmailToAll")]
        [HttpPost]
        public IHttpActionResult SendRegistrationEMailToAll()
        {                        
            _commandDispatcher.Dispatch( new SendRegistrationEmailToAllCommand() );
            return Ok();
        }
    }

}
