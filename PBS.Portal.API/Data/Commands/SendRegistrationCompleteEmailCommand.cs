using System;
using PBS.Core.CQRS.Command;

namespace PBS.Portal.API.Data.Commands
{
    public class SendRegistrationCompleteEmailCommand : ICommand
    {
        public string Email { get; set; }
    }
}
