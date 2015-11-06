using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class DeletePendingUserByGuidCommandHandler : CommandHandlerBase, ICommandHandler<DeletePendingUserByGuidCommand>
    {
        public DeletePendingUserByGuidCommandHandler(IApplicationDbContext context) : base(context)
        {
        }

        public void Execute(DeletePendingUserByGuidCommand command)
        {
            var pendingUser = Context.PendingUsers.First(p => p.Id == command.PendingUserId);

            if (pendingUser == null)
            {
                throw new ArgumentException(string.Format("PendingUser:{0} not found in PendingUsers table.", command.PendingUserId));
            }

            Context.PendingUsers.Remove(pendingUser);
            Context.Context.SaveChanges();
        }
    }
}
