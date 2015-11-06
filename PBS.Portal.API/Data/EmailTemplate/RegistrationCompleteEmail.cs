using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using PBS.Portal.API.DataModel;
using System.Web.Mvc;
using PBS.Portal.API.Controllers;
using System.IO;

namespace PBS.Portal.API.Data.EmailTemplate
{
    public static class RegistrationCompleteEmail
    {
        public const string SenderAddress = "info@peninsula-uk.com";
        public const string Subject = "Thank you for registering for the Peninsula Client Portal";
        private static readonly string LoginUrl = ConfigurationManager.AppSettings["LoginURL"];

        private static string _emailBody = @"
            Dear %CLIENTNAME%,<br><br>
            
            Thank you for registering for the Peninsula Client Portal.<br><br>

            You can access the Portal at any time by entering your email address and the password you have created at %LOGINLINK%.<br><br>

            If you have any questions, please contact the Client Satisfaction Team on 0844 892 2775.<br><br>
            
            Many thanks for your kind attention to this matter.<br><br>
            Your Peninsula Team.";

        private const string ClientNameTag = "%CLIENTNAME%";
        private const string LoginLink = "%LOGINLINK%";
        private const string EmailContentTag = "%EMailContent%";

        public static string GetEmailBody(PendingUser pendingUser)
        {
            return _emailBody
                .Replace(ClientNameTag, pendingUser.FirstName + " " + pendingUser.LastName)
                .Replace(LoginLink, LoginUrl);
        }

        public static string ApplyHtmlToEmailBody(PendingUser pendingUser)
        {
            // would MUCH RATHER have pulled in a string from a .HTML file here to make for easier changes in future but for now...
            string html = EmailHtmlTemplate.EmailContent;
            var finalEmailBody = GetEmailBody(pendingUser);

            return html.Replace(EmailContentTag, finalEmailBody);
        }
    };

}