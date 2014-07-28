using SDService.Model;
using ServiceLayer;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs/{jobName}/report/{buildName}")]
    public class JobReportsController : ApiController
    {
        JobStatusReportService reportService;

        public JobReportsController()
        {
            reportService = new JobStatusReportService();
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage FetchReport(string jobName, string buildName, [FromUri]string offset = "0")
        {
            JobReport report = reportService.getReport(jobName, buildName, offset);
            var responseMessage = Request.CreateResponse<JobReportToData>(HttpStatusCode.Accepted, new JobReportToData() { JobName = jobName, Completed = report.Completed.ToString(), Report = report.Report, Offset = report.Offset.ToString() });

            return responseMessage;
        }

        [Route("file")]
        [HttpGet]
        public HttpResponseMessage FetchReportInFile(string jobName, string buildName, [FromUri]string offset = "0")
        {
            JobReport report = reportService.getReport(jobName, buildName, offset);
           // var responseMessage = Request.CreateResponse<JobReportToData>(HttpStatusCode.Accepted, new JobReportToData() { JobName = jobName, Completed = report.Completed.ToString(), Report = report.Report, Offset = report.Offset.ToString() });
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(report.Report);
            writer.Flush();
            stream.Position = 0;
            
            HttpResponseMessage result = new HttpResponseMessage();
            result.StatusCode = HttpStatusCode.OK ;
            result.Content = new StreamContent(stream);
            //a text file is actually an octet-stream (pdf, etc)
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            //we used attachment to force download
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = report.JobName + "_build" + buildName + "_Report.txt";
            return result;


            //return responseMessage;
        }
    }
}
