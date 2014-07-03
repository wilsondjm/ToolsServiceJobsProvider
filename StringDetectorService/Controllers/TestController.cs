using ServiceLayer;
//#if DEGUB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
    public class TestController : ApiController
    {
        private PasswordEncryptionService pService;

        public TestController()
        {
            pService = new PasswordEncryptionService();
        }

        [Route("api/test")]
        [HttpGet]
        public IHttpActionResult test()
        {
            string result = pService.encryptString("1234abcD", HttpContext.Current.Server.MapPath(""), "");

            return Ok(result);
        }
    }
}

//#endif