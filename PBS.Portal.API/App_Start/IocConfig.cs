using PBS.Portal.API.Common;
using StructureMap;

namespace PBS.Portal.API
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