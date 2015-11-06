using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBS.Advice.API.Common;
using PBS.Logging;
using Registry = StructureMap.Configuration.DSL.Registry;


namespace PBS.Advice.DataModel.Common
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<IAdviceDbContext>().Use<AdviceDbContext>();


            For<ILoggingService>()
               .Use<LoggingService>()
               .Ctor<string>("applicationName").Is("PBS.Advice.API");


           



        }

    }
}
