using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class RegisterCommandHandler : CommandHandlerBase, ICommandHandler<RegisterCommand>
    {
        public RegisterCommandHandler(IApplicationDbContext context) : base(context)
        {
        }

        // @todo can we push all the AccountController.Register logic into here?
        public void Execute(RegisterCommand command)
        {
            var passwordHash = new PasswordHasher().HashPassword(command.Password);
            var aspNetUserId = Guid.NewGuid().ToString();
            var securityStamp = Guid.NewGuid().ToString();

            var aspNetUserClaim = new AspNetUserClaim
            {
                ClaimType = "PBS.ClientId",
                ClaimValue = command.ClientId.ToString(),
                UserId = aspNetUserId,
                AspNetUser = new AspNetUser
                {
                    Id = aspNetUserId,
                    PasswordHash = passwordHash,
                    Email = command.Email,
                    SecurityStamp = securityStamp,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = command.Email,
                    UserProfile = new UserProfile
                    {
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        CreatedDate = DateTime.Now
                    }
                }
            };

            Context.Claims.Add(aspNetUserClaim);
            Context.Context.SaveChanges();
        }
    }
}
