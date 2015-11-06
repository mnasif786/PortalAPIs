using PBS.Core.CQRS.Query;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Data.Queries
{
    public class GetPeninsulaClientIdByCanQuery : IQuery<PeninsulaClient>
    {
        public string CAN { get; set; }
    }
}