using System.Collections.Generic;
using PBS.Core.CQRS.Query;
using PBS.PortalAdmin.API.Models;


namespace PBS.PortalAdmin.API.Data.Queries
{
    public class GetPortalUsersQuery : IQuery<IList<PortalUserViewModel>>
    {
        public string CanOrCompanyName { get; set; }
        
    }
}