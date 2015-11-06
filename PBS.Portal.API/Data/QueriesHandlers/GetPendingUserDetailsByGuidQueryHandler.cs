using System;
using System.Linq;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.QueriesHandlers
{
    public class GetPendingUserDetailsByGuidQueryHandler : QueryHandlerBase, IQueryHandler<GetPendingUserDetailsByGuidQuery, RegisterViewModel>
    {
        public GetPendingUserDetailsByGuidQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public RegisterViewModel Execute(GetPendingUserDetailsByGuidQuery query)
        {
            var pendingUser = Context.PendingUsers.SingleOrDefault(x => x.Id.ToString() == query.PendingUserId.ToString());

            if (pendingUser == null)
            {
                // let the controller send Jack a specific message rather than a 500
                // throw new ArgumentException(string.Format("PendingUser:{0} not found in PendingUsers table.", query.PendingUserId));
                // but the 500 logs error to ErrorLog...!
            };

            return pendingUser.ToRegisterViewModel();
        }
    }
}