using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PBS.AuthorisationServer.API.Models
{
    public class ResourceBindingModel
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}