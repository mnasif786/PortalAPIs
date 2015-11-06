using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.PortalAdmin.API.DataModel;
using PBS.PortalAdmin.API.Models;

namespace PBS.PortalAdmin.API.Extensions
{
    public static class PortalUserViewModelExtension
    {
        public static PortalUserViewModel ToPortalUserViewModel(this AspNetUser userEntity, PeninsulaClient peninsulaClientEntity)
        {
            var portalUserViewModel = userEntity == null ? null : new PortalUserViewModel
            {
                UserId = userEntity.Id,
                Email = userEntity.UserName,
                FirstName = (userEntity.UserProfile != null) ? userEntity.UserProfile.FirstName : null,
                LastName = (userEntity.UserProfile != null) ? userEntity.UserProfile.LastName : null,
                ClientId = peninsulaClientEntity.ClientId,
                Can = peninsulaClientEntity.CAN,
                Company = peninsulaClientEntity.CompanyName
            };
            
            return portalUserViewModel;
        }
    }
}