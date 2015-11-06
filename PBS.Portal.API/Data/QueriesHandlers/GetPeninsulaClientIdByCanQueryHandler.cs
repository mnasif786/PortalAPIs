using System.Linq;
using System.Runtime.Remoting.Contexts;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.QueriesHandlers
{
    public class GetPeninsulaClientIdByCanQueryHandler : QueryHandlerBase, IQueryHandler<GetPeninsulaClientIdByCanQuery, PeninsulaClient>
    {
        public GetPeninsulaClientIdByCanQueryHandler(IApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public PeninsulaClient Execute(GetPeninsulaClientIdByCanQuery query)
        {
            var peninsulaClient = Context.PeninsulaClients.SingleOrDefault(p => p.CAN == query.CAN);
            return peninsulaClient;
        }
    }
}