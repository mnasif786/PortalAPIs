using PBS.Documents.API.SharepointDocService;
using PBS.Logging;
using Registry = StructureMap.Configuration.DSL.Registry;


namespace PBS.Documents.API.Common
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<IDocumentService>()
                .Use<SharepointDocumentService>();

            For<ISpService>()
                .Use(constructor => new SpServiceClient());

            //For<ISpService>()
            //    .Use<SpServiceClient>().SelectConstructor();

            For<ILoggingService>()
               .Use<LoggingService>()
               .Ctor<string>("applicationName").Is("PBS.Documents.API");
        }
    }
}