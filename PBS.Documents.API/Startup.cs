using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Cors;
using Owin;
using PBS.Core.DI;
using PBS.Logging;

namespace PBS.Documents.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            var container = IocConfig.Setup();
            config.DependencyResolver = new DependencyResolver(container);

            config.Services.Add(typeof(IExceptionLogger), new DocumentsExceptionLogger(container.GetInstance<ILoggingService>()));

            // Web API routes
            config.MapHttpAttributeRoutes();
            ConfigureOAuth(app);


            //http://benfoster.io/blog/aspnet-webapi-cors
            //To Allow CROSS-ORIGIN requests Globally at application level 
            var corsPolicy = new CorsPolicy { AllowAnyMethod = true, AllowAnyHeader = true, SupportsCredentials = true };
            var corsSettings = (NameValueCollection)ConfigurationManager.GetSection("CorsSettings");
            var allowedOrigins = corsSettings["AllowedOrigins"];

            if (allowedOrigins != null)
            {
                foreach (var origin in allowedOrigins.Split(';'))
                {
                    corsPolicy.Origins.Add(origin);
                }
            }
            else
            {
                corsPolicy.AllowAnyOrigin = true;
            }

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(corsPolicy)
                }
            };

            app.UseCors(corsOptions);



            app.UseWebApi(config);
        }
    }
}
