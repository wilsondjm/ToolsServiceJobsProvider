using PartialResponse.Net.Http.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace StringDetectorService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // add partial response formatter
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new PartialJsonMediaTypeFormatter() { IgnoreCase = true });
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
