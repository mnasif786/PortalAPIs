using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using PBS.Portal.API.Common;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Data.EmailTemplate
{
    public class PasswordResetEmail
    {
        private static string _emailBody = @"
                    Dear %CLIENTNAME%,
                    
                    Please click the link below, or copy and paste it into your browser, to reset your password for the Peninsula Client Portal.<br><br>
				
				    %PASSWORDRESETLINK%
                    <br><br>
						
				    If you have not requested to reset your password, it's likely that another user entered your email address by mistake while trying to reset a password. If you didn't initiate the request, you don't need to take any further action and can safely disregard this email.<br><br>

                    If you have any questions, please contact the Client Satisfaction Team on 0844 892 2775. <br><br>

                    Many thanks for your kind attention to this matter. <br><br>

				    Your Peninsula team.";


        private const string ClientNameTag = "%CLIENTNAME%";
        private const string EmailContentTag = "%EMailContent%";
        private const string PasswordResetTag = "%PASSWORDRESETLINK%";
        public const string SenderAddress = "info@peninsula-uk.com";
        public const string Subject = "Reset Your Password";
       
        private static string GetEmailBody(AspNetUser user)
        {
           return _emailBody
                .Replace(ClientNameTag, user.UserProfile.FirstName + " " + user.UserProfile.LastName)
                .Replace(PasswordResetTag, string.Format("{0}/uid={1}", ConfigurationManager.AppSettings["PasswordResetURL"], Helpers.Base64ForUrlEncode(user.Id)));
            
           
        }

        public static string ApplyHtmlToEmailBody(AspNetUser user)
        {
            // would MUCH RATHER have pulled in a string from a .HTML file here to make for easier changes in future but for now...
            var html = EmailHtmlTemplate.EmailContent;
            var finalEmailBody = GetEmailBody(user);

            return html.Replace(EmailContentTag, finalEmailBody);
        }
    }
}