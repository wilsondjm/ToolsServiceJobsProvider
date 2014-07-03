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
    public class JobHistoryController : ApiController
    {
        private JobHistoryService historyService;

        public JobHistoryController()
        {
            historyService = new JobHistoryService();
        }

        [Route("api/Jobs/{jobName}/History")]
        [HttpGet]
        public JobHistory getJobHistory(string jobName)
        {
            return historyService.getAllJobHistory(jobName);
        }
    }
}
