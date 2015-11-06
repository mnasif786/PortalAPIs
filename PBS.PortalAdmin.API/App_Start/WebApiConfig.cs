using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using PBS.Core.DI;
using PBS.Logging;

namespace PBS.PortalAdmin.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = IocConfig.Setup();
            config.DependencyResolver = new DependencyResolver(container);
            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger(container.GetInstance<ILoggingService>()));

            //CORS enabling logic is written in Glbal.asax file.
            
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
