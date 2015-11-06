using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Logging;
using StructureMap.Configuration.DSL;

namespace PBS.AuthorisationServer.API.Common
{
    public class AppRegistry: Registry
    {
        public AppRegistry()
        {
            For<ILoggingService>()
                .Use<LoggingService>()
                .Ctor<string>("applicationName").Is("PBS.AuthorisationServer.API");

        }

        
    }
}