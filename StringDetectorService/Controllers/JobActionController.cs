using SDService.Model;
using SDService.Model.Utils;
using ServiceLayer;
using StringDetectorService.Hubs;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StringDetectorService.Controllers
{
    public class JobActionController : ApiControllerWithHub<JobHub>
    {
        JobActionService jobActionService;
        JobsService jobsService;

        public JobActionController()
        {
            jobActionService = new JobActionService();
            jobsService = new JobsService();
        }

        [Route("api/Jobs/{JobName}/start")]
        [HttpPost]
        public HttpResponseMessage StartJob(string JobName,bool realtime =false)
        {
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
            bool result = jobActionService.startJob(JobName);
            if (result)
            {
               Job  job= jobsService.GetJob(JobName,fields);
               JobInfoToData responseData =
                               new JobInfoToData()
                               {
                                   jobName = job.JobName,
                                   setting = job.JobSettings,
                                   configuration = job.Configuration,
                                   builds = job.Builds,
                                   report = job.LastBuild,
                                   status = job.Status,
                               };
               if (realtime)
               {
                   // we will try to set partital response for real time next version
                   Hub.Clients.All.startJobCallBack(responseData);
               }
               return Request.CreateResponse(HttpStatusCode.OK,responseData);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest,Constants.StartActionFailed);
        }

        [Route("api/Jobs/{JobName}/stop")]
        [HttpDelete]
        public HttpResponseMessage StopJob(string JobName,bool realtime =false)
        {
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
            bool result = jobActionService.stopJob(JobName);
            if (result)
            {
                Job job = jobsService.GetJob(JobName,fields);
                JobInfoToData responseData =
                                new JobInfoToData()
                                {
                                    jobName = job.JobName,
                                    setting = job.JobSettings,
                                    configuration = job.Configuration,
                                    builds = job.Builds,
                                    report = job.LastBuild,
                                    status = job.Status,
                                };
                if (realtime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.stopJobCallBack(responseData);
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.StopActionFailed);
        }
    }
}