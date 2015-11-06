using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Models;
using PBS.Portal.API.Extensions;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class SendPasswordResetEmailCommandHandler : CommandHandlerBase, ICommandHandler<SendPasswordResetEmailCommand,CommandResultViewModel>
    {
        public SendPasswordResetEmailCommandHandler(IApplicationDbContext context) : base(context)
        {
        }

        public CommandResultViewModel Execute(SendPasswordResetEmailCommand command)
        {
            var commandResultViewModel = CommandResultViewModel.Create();
            var user = Context.Users.SingleOrDefault(u => u.Email.ToUpper().Equals(command.RecipientEmailAddress.ToUpper()));

            if (user == null)
            {
                commandResultViewModel.Message = "User Not Found";
                return commandResultViewModel;
            }

            user.SendPasswordResetEmail(); // Send out the email
            
            commandResultViewModel.CommandExecuted = true;
            return commandResultViewModel;
        }
    }
}