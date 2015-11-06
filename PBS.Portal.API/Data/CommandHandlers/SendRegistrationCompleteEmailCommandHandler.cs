using System;
using System.Linq;
using NServiceBus;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.EmailTemplate;
using PbsEmailer.Contracts;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class SendRegistrationCompleteEmailCommandHandler : CommandHandlerBase, ICommandHandler<SendRegistrationCompleteEmailCommand>
    {
        public SendRegistrationCompleteEmailCommandHandler(IApplicationDbContext dbContext) 
            :base(dbContext)
        {
            
        }
     
        public void Execute(SendRegistrationCompleteEmailCommand command)
        {
            var registeredUser = Context.PendingUsers.FirstOrDefault(u => u.Email == command.Email);

            if (registeredUser == null)
            {
                throw new ArgumentException( "User '" + command.Email + "' not found" );
            };

            registeredUser.SendRegistrationCompleteEmail();
        }   
    }
}