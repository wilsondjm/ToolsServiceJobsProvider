using ServiceProvider.Model;
using ServiceProvider.Model.Basic;
using ServiceProvider.Model.Utils;
using ServiceProvider.Service;
using StringDetectorService.Hubs;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using System.Web.Http.Cors;

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobsController : ApiControllerWithHub<JobHub>
    {
        JobsService jobsService;

        public JobsController()
        {
            jobsService = new JobsService();
        }
        
        [Route("")]
        [HttpGet]
        public IEnumerable<JobDto> getAllJobs()
        {
           Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);

            IEnumerable<Job> Jobs = jobsService.GetAllJobs(fields);
            IEnumerable<JobDto> responseData =  Jobs.Select(job =>job.ToJobDto()
                
                );

            return responseData;
        }


        [Route("{jobName}")]
        [HttpGet]
        public JobDto getJob(string jobName)
        {
           // Hub.Clients.User(userId)
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
            Job job = jobsService.GetJob(jobName,fields);
            JobDto responseData =job.ToJobDto();
           // Hub.Clients.All.hello("ok");
            return responseData;
        }

       

        [Route("{jobName}")]
        [HttpPost]
        public HttpResponseMessage createJob(string jobName, CreateJobData createJobData, bool realTime = false,string connectionId="")
           {
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);

            if (jobsService.CreateJob(createJobData.JobName,createJobData.UpstreamProject,Constants.DefaultConfiguration))
            {
                Job job = jobsService.GetJob(jobName,fields);
                JobDto responseData = job.ToJobDto();
                if (realTime&&connectionId!="")
                {
                    // for the partial response reason ,some request will not require job name filed ,we will add it by hand.
                    responseData.JobName = jobName;
                    Hub.Clients.AllExcept(connectionId).addJobCallBack(responseData);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.CreateActionFailed);
        }

       

        [Route("{jobName}")]
        [HttpDelete]
        public HttpResponseMessage deleteJob(string jobName,bool realTime=false,string connectionId="")
        {
            if(jobsService.deleteJob(jobName))
            {
                if (realTime&&connectionId!="")
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.AllExcept(connectionId).deleteJobCallBack(jobName);

                }
                return Request.CreateResponse(HttpStatusCode.OK, jobName);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.DeleteActionFailed);
        }
    }
}
