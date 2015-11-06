using System;
using PBS.Core.CQRS.Command;

namespace PBS.Portal.API.Data.Commands
{
    public class RegisterCommand : ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long ClientId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
