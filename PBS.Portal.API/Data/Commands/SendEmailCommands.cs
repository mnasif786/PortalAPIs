using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Command;

namespace PBS.Portal.API.Data.Commands
{

    public interface ISendEmailCommand
    {
        string RecipientEmailAddress { get; set; }
    }


    public class SendPasswordResetEmailCommand : ISendEmailCommand, ICommand
    {
        [Required]
        public string RecipientEmailAddress { get; set; }
    }

}