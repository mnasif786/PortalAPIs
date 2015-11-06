using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBS.Portal.API.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public UserProfileViewModel Profile { get; set; }
        public long ClientId { get; set; }
        public string Email { get; set; }
    }
}