using System;
using System.Collections.Generic;
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
    public class SendRegistrationEmailToAllCommandHandler : CommandHandlerBase, ICommandHandler<SendRegistrationEmailToAllCommand>
    {       
        public SendRegistrationEmailToAllCommandHandler(IApplicationDbContext dbContext) 
            :base(dbContext)
        {               
        }

        public void Execute(SendRegistrationEmailToAllCommand command)
        {           
            List<PendingUser> pendingUsers = Context.PendingUsers.ToList();

            foreach (PendingUser pu in pendingUsers)
            {                
                pu.SendRegistrationEmail();
            }
        }
    }
}