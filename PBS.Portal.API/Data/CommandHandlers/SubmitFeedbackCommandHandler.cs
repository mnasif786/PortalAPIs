using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using PBS.Core.CQRS.Command;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public class SubmitFeedbackCommandHandler : CommandHandlerBase, ICommandHandler<SubmitFeedbackCommand>
    {
        public SubmitFeedbackCommandHandler(IApplicationDbContext dbContext) 
            :base(dbContext)
        {
            
        }
     
        public void Execute(SubmitFeedbackCommand command)
        {
            // get user id from username           
            AspNetUser user = Context.Users.FirstOrDefault(x => x.UserName == ClaimsPrincipal.Current.Identity.Name);
            if (user == null)
            {
                throw new ArgumentException("No Such User: " + ClaimsPrincipal.Current.Identity.Name);
            }

            // create feedback
            Feedback feedback = new Feedback()
            {
                DateSubmitted = DateTime.Now,
                PortalUserID = user.Id,
                FeedbackText = command.FeedbackText,
                PageSubmittedFrom = command.Page
            };


            Context.Feedback.Add( feedback );
            Context.Context.SaveChanges();

        }
    }
}