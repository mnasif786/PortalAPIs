using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Data.QueriesHandlers;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.QueriesHandlers
{
    
    public class GetUserProfileByUserNameQueryHandler: QueryHandlerBase , IQueryHandler<GetUserProfileByUserNameQuery, UserViewModel>
    {
        public GetUserProfileByUserNameQueryHandler(IApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public UserViewModel Execute(GetUserProfileByUserNameQuery query)
        {
            var user = Context.Users.SingleOrDefault(u => u.UserName.Equals(query.UserName));            

            return user.ToUserClientViewModel();
        }
    }
} 