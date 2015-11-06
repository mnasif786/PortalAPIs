
using PBS.Client.API.Common;
using PBS.Client.API.Data.Queries;
using PBS.Client.API.Data.QueriesHandlers;
using PBS.Client.API.Models;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Logging;

using Registry = StructureMap.Configuration.DSL.Registry;


namespace PBS.Client.DataModel.Common
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<IQueryDispatcher>().Use<QueryDispatcher>();
            For<ICommandDispatcher>().Use<CommandDispatcher>();

            For<IQueryHandler<GetClientDetailsByClientIDQuery, ClientDetailsViewModel>>().Use<GetClientDetailsByClientIDQueryHandler>();
            //For<ICommandHandler<SubmitFeedbackCommand>>().Use<SubmitFeedbackCommandHandler>();

            For<IClientDbContext>().Use<ClientDbContext>();


            For<ILoggingService>()
               .Use<LoggingService>()
               .Ctor<string>("applicationName").Is("PBS.Client.API");


           



        }

    }
}
