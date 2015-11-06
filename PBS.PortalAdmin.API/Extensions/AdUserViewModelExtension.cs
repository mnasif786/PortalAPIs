using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using PBS.PortalAdmin.API.DataModel;
using PBS.PortalAdmin.API.Models;

namespace PBS.PortalAdmin.API.Extensions
{
    public static class AdUserViewModelExtension
    {
        public static AdUserViewModel ToAdUserViewModel(this IPrincipal user)
        {
            if (user != null && user.Identity != null)
            {
                return new AdUserViewModel
                {
                    DomainName = user.Identity.Name,
                    Name = string.IsNullOrEmpty(user.Identity.Name) ? "" : user.Identity.Name.Split('\\')[1].Replace(".", " "),
                    IsAuthenticated = user.Identity.IsAuthenticated
                };
            }

            return null;
        }
    }
}