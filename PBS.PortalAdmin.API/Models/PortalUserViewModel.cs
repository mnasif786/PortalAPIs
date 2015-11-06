using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBS.PortalAdmin.API.Models
{
    public class PortalUserViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Can { get; set; }
        public string Company { get; set; }
        public long ClientId { get; set; }
    }
}