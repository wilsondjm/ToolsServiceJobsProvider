using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
    public class JobActionController : ApiController
    {
        JobActionService jobActionService;

        public JobActionController()
        {
            jobActionService = new JobActionService();
        }

        [Route("api/Jobs/{JobName}/start")]
        [HttpPost]
        public IHttpActionResult StartJob(string JobName)
        {
            bool result = jobActionService.startJob(JobName);
            if (result)
                return Ok("Job created");
            return BadRequest("Action Failed");
        }

        [Route("api/Jobs/{JobName}/stop")]
        [HttpDelete]
        public IHttpActionResult StopJob(string JobName)
        {
            bool result = jobActionService.stopJob(JobName);
            if (result)
                return Ok("Job Stoped");
            return BadRequest();
        }
    }
}