using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using NUnit.Framework;
using PBS.Portal.API.Data.EmailTemplate;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;

namespace PBS.Portal.API.Tests.CommandHandlerTests
{
    [TestFixture]
    public class SendRegistrationCompleteEmailCommandHandlerTests
    {
        [Test]
        public void CanReadEmailTemplateFileFromApp()
        {
            var pendingUser = new PendingUser
            {
                CAN = "123",
                Id = Guid.NewGuid(),
                Email = "Andrew.Osiname@peninsula-uk.com",
                FirstName = "Freddie",
                LastName = "Llunjberg"
            };

            string html = RegistrationCompleteEmail.ApplyHtmlToEmailBody(pendingUser);
            Assert.IsTrue(html.Contains("Dear Freddie Llunjberg"));
            //pendingUser.SendRegistrationCompleteEmail();
        }
    }  
}
