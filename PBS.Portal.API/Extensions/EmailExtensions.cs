using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using NServiceBus;
using PBS.Portal.API.Data.EmailTemplate;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Models;
using PbsEmailer.Contracts;
using StructureMap;

namespace PBS.Portal.API.Extensions
{
    //NA: To Mock extension Methods we need to create extension Method wrapper interface and  extension Methods wrapper class that implements that interface
    //We need to do this because extension methods are static methods
    public interface IEmailExtenionsWrapper
    {
        void SendPasswordChangedEmail(AspNetUser user);
    }

    public class EmailExtensionsWrapper : IEmailExtenionsWrapper
    {
      
        public void SendPasswordChangedEmail(AspNetUser user)
        {
            var recipients = new[] { user.Email };
           
            var msg = new Message(recipients,
                                        null,
                                        null,
                                        PasswordChangedEmail.Subject,
                                        PasswordChangedEmail.ApplyHtmlToEmailBody(user),
                                        PasswordChangedEmail.SenderAddress);

            var busConfig = new BusConfiguration();
            using (var bus = Bus.CreateSendOnly(busConfig))
            {
                bus.Send(msg);
            }
        }
    }

    public static class EmailExtensions
    {
        
        public static IEmailExtenionsWrapper EmailExtensionWrapper = new EmailExtensionsWrapper(); 
        
        
        public static void SendRegistrationEmail(this PendingUser pendingUser)
        {
            string[] recipients = new[] { pendingUser.Email };
            string[] ccRecipients = null;
            string[] bccRecipients = null;

            Message msg = new Message(recipients,
                                        ccRecipients,
                                        bccRecipients,
                                        RegistrationEmail.Subject,
                                        RegistrationEmail.ApplyHtmlToEmailBody(pendingUser),
                                        RegistrationEmail.SenderAddress);

            BusConfiguration busConfig = new BusConfiguration();
            using (ISendOnlyBus bus = Bus.CreateSendOnly(busConfig))
            {
                bus.Send(msg);
            }
        }

        public static void SendRegistrationCompleteEmail(this PendingUser pendingUser)
        {
            string[] recipients = new[] { pendingUser.Email };
            string[] ccRecipients = null;
            string[] bccRecipients = null;

            Message msg = new Message(recipients,
                                        ccRecipients,
                                        bccRecipients,
                                        RegistrationCompleteEmail.Subject,
                                        RegistrationCompleteEmail.ApplyHtmlToEmailBody(pendingUser),
                                        RegistrationCompleteEmail.SenderAddress);

            BusConfiguration busConfig = new BusConfiguration();
            using (ISendOnlyBus bus = Bus.CreateSendOnly(busConfig))
            {
                bus.Send(msg);
            }
        }

        public static void SendPasswordResetEmail(this AspNetUser user)
        {
            var recipients = new[] { user.Email };
            //string[] ccRecipients = null;
            //string[] bccRecipients = null;

            var msg = new Message(recipients,
                                        null,
                                        null,
                                        PasswordResetEmail.Subject,
                                        PasswordResetEmail.ApplyHtmlToEmailBody(user),
                                        PasswordResetEmail.SenderAddress);

            var busConfig = new BusConfiguration();
            using (var bus = Bus.CreateSendOnly(busConfig))
            {
                bus.Send(msg);
            }
        }

        public static void SendPasswordChangedEmail(this AspNetUser user)
        {
            EmailExtensionWrapper.SendPasswordChangedEmail(user);
        }
    }
}