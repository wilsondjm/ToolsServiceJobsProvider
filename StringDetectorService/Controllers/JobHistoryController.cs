using SDService.Model.Basic;
using ServiceLayer;
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
    public class JobHistoryController : ApiControllerWithHub<JobHub>
    {
        private JobHistoryService historyService;

        public JobHistoryController()
        {
            historyService = new JobHistoryService();
        }

        [Route("api/Jobs/{jobName}/History")]
        [HttpGet]
        public JobHistoryDto getJobHistory(string jobName)
        {
            return historyService.getAllJobHistory(jobName).ToJobHistoryDto();
        }
    }
}
