using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.AuthorisationServer.API.Common;
using StructureMap;

namespace PBS.AuthorisationServer.API
{
    public class IocConfig
    {
        public static IContainer Setup()
        {

            return new Container(x =>
            {
                x.AddRegistry<AppRegistry>();

                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.LookForRegistries();

                });
            });
        }
    }
}