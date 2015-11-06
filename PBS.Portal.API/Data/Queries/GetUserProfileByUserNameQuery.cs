using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Data.Queries
{
    public class GetUserProfileByUserNameQuery: IQuery<UserViewModel>
    {
        public string UserName { get; set; }
    }
}