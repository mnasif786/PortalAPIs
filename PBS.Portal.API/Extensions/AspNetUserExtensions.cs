using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Extensions
{
    public static class AspNetUserExtensions
    {
        private static readonly string regEmailUrl = ConfigurationManager.AppSettings["RegistrationEmailURL"];

        public static string EntryPointURL(this AspNetUser user)
        {
            return String.Format("{0}/{1}={2}", regEmailUrl, "uid", user.Id );
        }
    }
}

