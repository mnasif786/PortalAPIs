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
    public static class PasswordChangedEmail
    {
        public const string SenderAddress = "info@peninsula-uk.com";
        public const string Subject = "Password Changed Confirmation";
        

        private static string _emailBody = @"
            Dear %CLIENTNAME%,<br><br>
            
            The password for your Peninsula Client Portal account has recently been changed.<br><br>

            If you haven’t recently changed your password we would recommend that you use the forgotten password link to reset it.<br><br>

            If you have any questions, please contact the Client Satisfaction Team on 0844 892 2775.<br><br>
            
            Many thanks for your kind attention to this matter.<br><br>
            Your Peninsula Team.";

        private const string ClientNameTag = "%CLIENTNAME%";
        private const string EmailContentTag = "%EMailContent%";

        public static string GetEmailBody(AspNetUser user)
        {
            return _emailBody
                .Replace(ClientNameTag, user.UserProfile.FirstName + " " + user.UserProfile.LastName);
        }

        public static string ApplyHtmlToEmailBody(AspNetUser user)
        {
            // would MUCH RATHER have pulled in a string from a .HTML file here to make for easier changes in future but for now...
            string html = EmailHtmlTemplate.EmailContent;
            var finalEmailBody = GetEmailBody(user);

            return html.Replace(EmailContentTag, finalEmailBody);
        }
    };

}