using PBS.PortalAdmin.API.Common;
using StructureMap;

namespace PBS.PortalAdmin.API
{
    public static class IocConfig
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