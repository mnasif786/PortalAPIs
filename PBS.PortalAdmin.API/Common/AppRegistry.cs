using System.Collections.Generic;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Logging;
using PBS.PortalAdmin.API.Data.Queries;
using PBS.PortalAdmin.API.Data.QueriesHandlers;
using PBS.PortalAdmin.API.Models;
using StructureMap.Configuration.DSL;

namespace PBS.PortalAdmin.API.Common
{
    public class AppRegistry:Registry
    {
        public AppRegistry()
        {
            For<IQueryDispatcher>().Use<QueryDispatcher>();
            For<ICommandDispatcher>().Use<CommandDispatcher>();


            For<IQueryHandler<GetPortalUsersQuery, IList<PortalUserViewModel>>>().Use<GetPortalUsersQueryHandler>();
            For<IQueryHandler<ClientRegisterationVerificationQuery, bool>>().Use<ClientRegisterationVerificationQueryHandler>();
            
            For<IApplicationDbContext>().Use<ApplicationDbContext>();

            For<ILoggingService>()
              .Use<LoggingService>()
              .Ctor<string>("applicationName").Is("PBS.PortalAdmin.API");
        }
    }
}