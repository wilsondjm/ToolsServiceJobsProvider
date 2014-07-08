using ServiceLayer;
//#if DEGUB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace StringDetectorService.Controllers
{
    public class TestController : ApiController
    {
        public TestController()
        {
        }

        [Route("api/test")]
        [HttpGet]
        public IHttpActionResult test()
        {
            Configuration.Services.GetTraceWriter().Trace(Request, "Incoming Request", TraceLevel.Debug, "Update the config file");
            return Ok();
        }
    }
}

//#endif