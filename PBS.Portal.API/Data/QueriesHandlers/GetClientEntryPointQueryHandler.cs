using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.QueriesHandlers
{
    
        public class GetClientEntryPointQueryHandler : QueryHandlerBase, IQueryHandler<GetClientEntryPointQuery,EntryPointURLModel>
        {
            public GetClientEntryPointQueryHandler(IApplicationDbContext dbContext)
                : base(dbContext)
            {

            }

            public virtual EntryPointURLModel Execute(GetClientEntryPointQuery query)
            {                
                EntryPointURLModel model = new EntryPointURLModel(String.Empty);
                
                // is there a pending user for this CAN?
                PendingUser pendingUser = Context.PendingUsers.FirstOrDefault(u => u.CAN.Equals(query.CAN));

                // If so, return URL
                if (pendingUser != null)
                {
                    model.URL = pendingUser.EntryPointURL();
                }
                else // is there a registered user for this can?
                {                                        
                    // get client id for can
                    PeninsulaClient client = Context.PeninsulaClients.FirstOrDefault(c => c.CAN == query.CAN);
                    if (client != null)
                    {
                        // checked claims for can
                        AspNetUserClaim claim = Context.Claims.FirstOrDefault(
                                                        cl =>   (cl.ClaimValue == client.ClientId.ToString()) &&
                                                                (cl.ClaimType == PBS.Claims.ClaimTypes.PBSClientId));

                        // if claim exist, get user
                        if (claim != null)
                        {
                            // generate URL
                            model.URL = claim.AspNetUser.EntryPointURL();
                        }
                    }
                }

                return model;
            }
        }
    
}