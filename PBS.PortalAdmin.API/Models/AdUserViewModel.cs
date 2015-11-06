using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBS.PortalAdmin.API.Models
{
    public class AdUserViewModel
    {
        public string DomainName { get; set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}