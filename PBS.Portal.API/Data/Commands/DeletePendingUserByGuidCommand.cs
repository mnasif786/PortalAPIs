using System;
using PBS.Core.CQRS.Command;

namespace PBS.Portal.API.Data.Commands
{
    public class DeletePendingUserByGuidCommand : ICommand
    {
        public Guid PendingUserId { get; set; }
    }
}
