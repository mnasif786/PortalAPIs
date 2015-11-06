using PBS.Client.API.Models;
using PBS.Core.CQRS.Query;


namespace PBS.Client.API.Data.Queries
{
    public class GetClientDetailsByClientIDQuery: IQuery<ClientDetailsViewModel>
    {
        public int ClientID { get; set; }
    }
}