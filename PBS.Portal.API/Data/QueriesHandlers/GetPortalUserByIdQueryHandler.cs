using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.QueriesHandlers
{
    public class GetPortalUserByIdQueryHandler : QueryHandlerBase, IQueryHandler<GetPortalUserByIdQuery, UserViewModel>
    {
        public GetPortalUserByIdQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public UserViewModel Execute(GetPortalUserByIdQuery query)
        {

            var user = Context.Users.SingleOrDefault(u => u.Id.Equals(query.UserId));

            return user.ToUserViewModel();
        }
    }
}