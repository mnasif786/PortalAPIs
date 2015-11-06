using PBS.Advice.DataModel.Common;
using StructureMap;

namespace PBS.Advice.API
{
    public static class IocConfig
    {
        public static IContainer Setup()
        {

            return new Container(x =>
            {
                x.AddRegistry<DataRegistry>();

                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.LookForRegistries();

                });
            });
        }

    }
}

