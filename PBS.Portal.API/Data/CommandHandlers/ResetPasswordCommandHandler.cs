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
    public class ResetPasswordCommandHandler : CommandHandlerBase, ICommandHandler<ResetPasswordCommand,CommandResultViewModel>
    {
        public ResetPasswordCommandHandler(IApplicationDbContext context)
            : base(context)
        {
        }


        public CommandResultViewModel Execute(ResetPasswordCommand command)
        {
            var commandResultViewModel = CommandResultViewModel.Create();
            
            var user = Context.Users.SingleOrDefault(u => u.Id.Equals(command.UserId));

            if (user == null)
            {
                commandResultViewModel.Message = "User Id Not Found";
                return commandResultViewModel;
            }

            user.ChangePassword(command.NewPassword);

            var result = Context.Save(); //Save changes to the database.

            if (result <= 0) //changed not made to database i.e. result <=0
            {
                commandResultViewModel.Message = "Reset Password Failed: Changes not committed to the database.";
                return commandResultViewModel;
            }
            

            commandResultViewModel.CommandExecuted = true;
            user.SendPasswordChangedEmail();

            return commandResultViewModel;
        }
    }
}