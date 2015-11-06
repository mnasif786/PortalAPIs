using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.PortalAdmin.API.Common;
using PBS.PortalAdmin.API.Data.Queries;

namespace PBS.PortalAdmin.API.Data.QueriesHandlers
{
    public class ClientRegisterationVerificationQueryHandler:QueryHandlerBase, IQueryHandler<ClientRegisterationVerificationQuery, bool>
    {
        public ClientRegisterationVerificationQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public bool Execute(ClientRegisterationVerificationQuery query)
        {
            
            var peninsulaClient = Context.PeninsulaClients.
                SingleOrDefault(x => x.ClientId.ToString().Equals(query.ClientIdOrCan) || x.CAN.ToLower().Equals(query.ClientIdOrCan.ToLower()));

            if (peninsulaClient == null) return false;

            var result = Context.Claims.Any(c => c.ClaimValue.Equals(peninsulaClient.ClientId.ToString())) || 
                                            Context.PendingUsers.Any(p => p.CAN.ToLower().Equals(peninsulaClient.CAN.ToLower()));


            return result;
        }
    }
}