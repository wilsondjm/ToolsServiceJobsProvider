using StringDetectorService.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Routing;

namespace StringDetectorService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // add web api CORS support
            config.EnableCors();

            // Web API configuration and services
            config.EnableSystemDiagnosticsTracing();
            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //enable customized trace writer
            config.Services.Replace(typeof(ITraceWriter), new CustomizedTraceWriter());
        }
    }
}
