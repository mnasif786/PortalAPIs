using System;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.Queries
{
    public class GetPendingUserDetailsByGuidQuery : IQuery<RegisterViewModel>
    {
        public Guid PendingUserId { get; set; }
    }
}
