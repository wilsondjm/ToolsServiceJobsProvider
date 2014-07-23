using SDService.Model.Basic;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
    public class JobStatusController : ApiController
    {
         private JobStatusService statusService;

         public JobStatusController()
        {
            statusService = new JobStatusService();
        }

        [Route("api/Jobs/{jobName}/status")]
        [HttpGet]
        public JobStatus getJobStatus(string jobName)
        {
            return statusService.getJobStatus(jobName);
        }
    }
}
