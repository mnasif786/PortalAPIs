using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NServiceBus;
using PBS.Portal.API.Data.EmailTemplate;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Extensions
{
    public static class PendingUserExtensions
    {
        private static readonly string regEmailUrl = ConfigurationManager.AppSettings["RegistrationEmailURL"];   
     
        public static string EntryPointURL(this PendingUser pendingUser)
        {
            return String.Format("{0}/{1}={2}", regEmailUrl, "uid", pendingUser.Id.ToString());
        }
    }
}