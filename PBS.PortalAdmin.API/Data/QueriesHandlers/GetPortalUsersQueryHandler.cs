using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.PortalAdmin.API.Common;
using PBS.PortalAdmin.API.Data.Queries;
using PBS.PortalAdmin.API.Extensions;
using PBS.PortalAdmin.API.Models;

namespace PBS.PortalAdmin.API.Data.QueriesHandlers
{
    public class GetPortalUsersQueryHandler : QueryHandlerBase, IQueryHandler<GetPortalUsersQuery, IList<PortalUserViewModel>>
    {
        public GetPortalUsersQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IList<PortalUserViewModel> Execute(GetPortalUsersQuery query)
        {
            var peninsulaClient = Context.PeninsulaClients.SingleOrDefault(x => x.CAN.ToLower().Equals(query.CanOrCompanyName.ToLower()) || x.CompanyName.ToLower().Equals(query.CanOrCompanyName.ToLower()));

            if (peninsulaClient == null) return null;

            var claims =  Context.Claims.Where(c => c.ClaimValue.Equals(peninsulaClient.ClientId.ToString())).ToList();
            var portalUserViewModelList = claims.Select(x => x.AspNetUser.ToPortalUserViewModel(peninsulaClient)).ToList();

            return portalUserViewModelList;
        }
    }
}