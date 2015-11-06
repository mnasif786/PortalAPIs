using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace PBS.Portal.API.DataModel
{
    public partial class AspNetUser
    {
        public void ChangePassword(string newPassword)
        {
            PasswordHash = new PasswordHasher().HashPassword(newPassword);
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}