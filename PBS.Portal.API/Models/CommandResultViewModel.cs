using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Command;

namespace PBS.Portal.API.Models
{
    public class CommandResultViewModel
    {
        public bool CommandExecuted { get; set; }
        public string Message { get; set; }

       
        public static CommandResultViewModel Create()
        {
            return new CommandResultViewModel();
        }
        
       

    }
}