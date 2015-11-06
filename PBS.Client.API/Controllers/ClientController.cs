using System;
using System.Web.Http;
using System.Web.Http.Cors;
using PBS.Claims.Extensions;
using PBS.Client.API.Data.Queries;
using PBS.Core.CQRS.Query;
using PBS.Claims;


namespace PBS.Client.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/client")]
  //  [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClientController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;


        public ClientController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;         
        }      

        [Route("{clientID}")]
        public IHttpActionResult GetClientDetailsByClientID([FromUri]GetClientDetailsByClientIDQuery qry)
        {
            try
            {
                if (qry == null)
                {
                    throw new ArgumentNullException("GetClientDetailsByClientIDQuery");
                }

                if (qry != null && !User.CanViewDetailsForClientId(qry.ClientID))
                {
                    return Unauthorized();
                }

                var result = _queryDispatcher.Dispatch(qry);

                return Ok(result);           
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

          
        }     
    }
}

