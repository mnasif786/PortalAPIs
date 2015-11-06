using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class ChangePasswordCommandHandler : CommandHandlerBase, ICommandHandler<ChangePasswordCommand,CommandResultViewModel>
    {
        public ChangePasswordCommandHandler(IApplicationDbContext context) : base(context)
        {
        }


        public CommandResultViewModel Execute(ChangePasswordCommand command)
        {
            var commandResultViewModel = CommandResultViewModel.Create();
            
            var user = Context.Users.SingleOrDefault(u => u.Id.Equals(command.UserId));

            if (user == null)
            {
                commandResultViewModel.Message = "User Id Not Found";
                return commandResultViewModel;
            }

            var passwordVerificationResult = new PasswordHasher().VerifyHashedPassword(user.PasswordHash, command.OldPassword);

            if (command.UserName != user.UserName || passwordVerificationResult != PasswordVerificationResult.Success)
            {
                commandResultViewModel.Message = "User credentials not verified";
                return commandResultViewModel;
            }

            user.ChangePassword(command.NewPassword);

            var result = Context.Save();

            if (result <= 0)
            {
                commandResultViewModel.Message = "Change Password Failed: Changes not committed to the database.";
                return commandResultViewModel;
            }

            user.SendPasswordChangedEmail();
            commandResultViewModel.CommandExecuted = true;
            
            return commandResultViewModel;
        }
    }
}