using System.Linq;
using PBS.Claims;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Extensions
{
    public static class ModelExtensions
    {
        public static UserViewModel ToUserClientViewModel(this AspNetUser userEntity)
        {
            long clientId = 0;

            if (userEntity != null && userEntity.AspNetUserClaims != null)
            {
                AspNetUserClaim claim =
                    userEntity.AspNetUserClaims.SingleOrDefault(cl => cl.ClaimType == ClaimTypes.PBSClientId);

                if (claim != null)
                    clientId = long.Parse(claim.ClaimValue);
            }


            var userViewModel = userEntity == null ? null : new UserViewModel
            {
                UserId = userEntity.Id,
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                Profile = userEntity.UserProfile.ToUserProfileViewModel(),
                ClientId = clientId
            };

            return userViewModel;
        }

        public static UserViewModel ToUserViewModel(this AspNetUser userEntity, long clientId)
        {
            var userViewModel = userEntity == null ? null : new UserViewModel
            {
                UserId = userEntity.Id,
                UserName = userEntity.UserName,
                ClientId = clientId,
                Profile = userEntity.UserProfile.ToUserProfileViewModel()
            };

            return userViewModel;
        }

        public static UserProfileViewModel ToUserProfileViewModel(this UserProfile userProfileEntity)
        {
            var userProfileViewModel = userProfileEntity == null ? null : new UserProfileViewModel
            {
                FirstName = userProfileEntity.FirstName,
                MiddleName = userProfileEntity.MiddleName,
                LastName = userProfileEntity.LastName
            };

            return userProfileViewModel;
        }

        public static RegisterViewModel ToRegisterViewModel(this PendingUser pendingUserEntity)
        {
            var registerViewModel = pendingUserEntity == null
                ? null
                : new RegisterViewModel
                {
                    Email = pendingUserEntity.Email,
                    CAN = pendingUserEntity.CAN,
                    FirstName = pendingUserEntity.FirstName,
                    LastName = pendingUserEntity.LastName,
                    PendingUserId = pendingUserEntity.Id
                };

            return registerViewModel;
        }

        public static UserViewModel ToUserViewModel(this AspNetUser userEntity)
        {
            var userViewModel = userEntity == null ? null : new UserViewModel
            {
                UserId = userEntity.Id,
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                Profile = userEntity.UserProfile.ToUserProfileViewModel()
            };

            return userViewModel;
        }
    }
}