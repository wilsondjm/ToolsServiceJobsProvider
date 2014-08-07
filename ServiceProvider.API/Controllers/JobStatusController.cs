using ServiceProvider.Model.Basic;
using ServiceProvider.Service;
using StringDetectorService.Hubs;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace StringDetectorService.Controllers
{
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobStatusController : ApiControllerWithHub<JobHub>
    {
         private JobStatusService statusService;

         public JobStatusController()
        {
            statusService = new JobStatusService();
        }

        [Route("api/Jobs/{jobName}/status")]
        [HttpGet]
        public JobStatusDto getJobStatus(string jobName)
        {
            return statusService.getJobStatus(jobName).ToJobStatusDto();
        }
    }
}
