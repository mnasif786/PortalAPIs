using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin.Cors;
using Owin;
using PBS.AuthorisationServer.API.Common;
using PBS.Logging;

namespace PBS.AuthorisationServer.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            
            //config.Services.Add(typeof(IExceptionLogger), NLogExceptionLogger.Create("PBS.AuthorisationServer.API"));
            var container = IocConfig.Setup();
            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger(container.GetInstance<ILoggingService>()));

            // Web API routes
            config.MapHttpAttributeRoutes();

            ConfigureOAuth(app);

            //http://benfoster.io/blog/aspnet-webapi-cors
            //To Allow CROSS-ORIGIN requests Globally at application level 
            //var corsPolicy = new CorsPolicy { AllowAnyMethod = true, AllowAnyHeader = true };
            //var corsSettings = (NameValueCollection)ConfigurationManager.GetSection("CorsSettings");
            //var allowedOrigins = corsSettings["AllowedOrigins"];

            //if (allowedOrigins != null)
            //{
            //    foreach (var origin in allowedOrigins.Split(';'))
            //    {
            //        corsPolicy.Origins.Add(origin);
            //    }
            //}
            //else
            //{
            //    corsPolicy.AllowAnyOrigin = true;
            //}

            //var corsOptions = new CorsOptions
            //{
            //    PolicyProvider = new CorsPolicyProvider
            //    {
            //        PolicyResolver = context => Task.FromResult(corsPolicy)
            //    }
            //};

            //app.UseCors(corsOptions);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(config);

        }
    }
}