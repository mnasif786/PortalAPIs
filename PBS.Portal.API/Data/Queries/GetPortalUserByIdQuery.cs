using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.Queries
{
    public class GetPortalUserByIdQuery : IQuery<UserViewModel>
    {
        [Required]
        public string UserId { get; set; }
    }
}