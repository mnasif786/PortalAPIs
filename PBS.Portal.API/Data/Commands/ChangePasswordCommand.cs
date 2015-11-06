using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;

namespace PBS.Portal.API.Data.Commands
{
    public class ChangePasswordCommand : ICommand
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string 
        OldPassword{ get; set; }
        
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*(_|[^\w])).{7,}.+$", ErrorMessage = "{0} must be at least 8 characters and contain lowercase, uppercase, a number and a special character.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}