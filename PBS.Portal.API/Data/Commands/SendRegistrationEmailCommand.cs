using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Command;

namespace PBS.Portal.API.Data.Commands
{
    public class SendRegistrationEmailCommand : ICommand
    {
        public string Email { get; set; }
    }
}