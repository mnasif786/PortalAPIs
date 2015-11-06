using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Logging;
using PBS.Portal.API.Data.CommandHandlers;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Data.QueriesHandlers;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;
using StructureMap.Configuration.DSL;

namespace PBS.Portal.API.Common
{
    public class AppRegistry:Registry
    {
        public AppRegistry()
        {           
        
            

            For<IQueryDispatcher>().Use<QueryDispatcher>();
            For<ICommandDispatcher>().Use<CommandDispatcher>();

            For<IQueryHandler<GetUserProfileByUserNameQuery,UserViewModel>>().Use<GetUserProfileByUserNameQueryHandler>();
            For<IQueryHandler<GetPendingUserDetailsByGuidQuery, RegisterViewModel>>().Use<GetPendingUserDetailsByGuidQueryHandler>();
            For<IQueryHandler<GetPeninsulaClientIdByCanQuery, PeninsulaClient>>().Use<GetPeninsulaClientIdByCanQueryHandler>();
            For<IQueryHandler<GetClientEntryPointQuery, EntryPointURLModel>>().Use<GetClientEntryPointQueryHandler>();
            For<IQueryHandler<GetPortalUserByIdQuery, UserViewModel>>().Use<GetPortalUserByIdQueryHandler>();


            For<ICommandHandler<SendRegistrationCompleteEmailCommand>>().Use<SendRegistrationCompleteEmailCommandHandler>();
            For<ICommandHandler<SendRegistrationEmailCommand>>().Use<SendRegistrationEmailCommandHandler>();
            For<ICommandHandler<SubmitFeedbackCommand>>().Use<SubmitFeedbackCommandHandler>();
            For<ICommandHandler<RegisterCommand>>().Use<RegisterCommandHandler>();
            For<ICommandHandler<DeletePendingUserByGuidCommand>>().Use<DeletePendingUserByGuidCommandHandler>();
            For<ICommandHandler<SendRegistrationEmailCommand>>().Use<SendRegistrationEmailCommandHandler>();
            For<ICommandHandler<SendRegistrationEmailToAllCommand>>().Use<SendRegistrationEmailToAllCommandHandler>();            
            For<ICommandHandler<ChangePasswordCommand,CommandResultViewModel>>().Use<ChangePasswordCommandHandler>();
            
            For<ICommandHandler<SendPasswordResetEmailCommand, CommandResultViewModel>>().Use<SendPasswordResetEmailCommandHandler>();
            For<IApplicationDbContext>().Use<ApplicationDbContext>();

            For<ILoggingService>()
              .Use<LoggingService>()
              .Ctor<string>("applicationName").Is("PBS.Portal.API");

            For<ICommandHandler<ResetPasswordCommand, CommandResultViewModel>>().Use<ResetPasswordCommandHandler>();
            For <IEmailExtenionsWrapper>().Use<EmailExtensionsWrapper>();
        }
    }
}