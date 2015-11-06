using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PBS.AuthorisationServer.API.Models;

namespace PBS.AuthorisationServer.API.Controllers
{
    [RoutePrefix("api/register/resource")]
    public class RegisterResourceController : ApiController
    {
        //[Route("")]
        [NonAction]
        public IHttpActionResult Post(ResourceBindingModel resourceModel)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Test exception");
                //return BadRequest(ModelState);
            }

            var newResource = ResourceStore.AddResource(resourceModel.Name);

            return Ok(newResource);

        }
    }
}
