using System;
using System.Linq;
using NServiceBus;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.EmailTemplate;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;
using PbsEmailer.Contracts;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class SendRegistrationEmailCommandHandler : CommandHandlerBase, ICommandHandler<SendRegistrationEmailCommand>
    {
        
        public SendRegistrationEmailCommandHandler(IApplicationDbContext dbContext) 
            :base(dbContext)
        {            
        }
     
        public void Execute(SendRegistrationEmailCommand command)
        {
            // get user
            PendingUser pendingUser = Context.PendingUsers.FirstOrDefault(p => p.Email == command.Email);
            if (pendingUser == null)
            {
                throw new ArgumentException( "User '" + command.Email + "' not found" );
            };
                           
            pendingUser.SendRegistrationEmail();
        }        
    }
}