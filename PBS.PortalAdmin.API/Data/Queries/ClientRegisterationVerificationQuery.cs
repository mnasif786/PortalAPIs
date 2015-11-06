using System.Collections.Generic;
using PBS.Core.CQRS.Query;
using PBS.PortalAdmin.API.Models;


namespace PBS.PortalAdmin.API.Data.Queries
{
    public class ClientRegisterationVerificationQuery : IQuery<IList<PortalUserViewModel>>, IQuery<bool>
    {
        public string ClientIdOrCan { get; set; }
        
    }
}