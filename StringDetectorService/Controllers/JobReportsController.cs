using SDService.Model;
using ServiceLayer;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobReportsController : ApiController
    {
        JobStatusReportService reportService;

        public JobReportsController()
        {
            reportService = new JobStatusReportService();
        }

        [Route("{jobName}/Report/{buildName}")]
        [HttpGet]
        public HttpResponseMessage FetchReport(string jobName, string buildName, [FromUri]string offset = "0")
        {
            JobReport report = reportService.getReport(jobName, buildName, offset);
            var responseMessage = Request.CreateResponse<JobReportToData>(HttpStatusCode.Accepted, new JobReportToData() { JobName = jobName, Completed = report.Completed.ToString(), Report = report.Report, Offset = report.Offset.ToString() });

            return responseMessage;
        }
    }
}
