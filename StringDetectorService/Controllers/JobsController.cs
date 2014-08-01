using SDService.Model;
using SDService.Model.Basic;
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
using System.Threading;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.SignalR;

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs")]
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
        public HttpResponseMessage createJob(string jobName, JobSettingDto jobSettingData,bool realTime=false)
        {
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
            SCMSetting scmSetting = new SCMSetting()
            {
                SCMPort = jobSettingData.SCMPort,
                UserName = jobSettingData.UserName,
                Password = new PasswordEncryptionService().encryptString(jobSettingData.Password, HttpContext.Current.Server.MapPath(""), "\\..\\.."),
                Workspace = jobSettingData.Workspace,
                ViewMap = jobSettingData.ViewMap
            };
            var scmList = new List<SCMSetting>(); scmList.Add(scmSetting);

            if (jobsService.CreateJob(new JobSetting()
            {
                JobName = jobName,
                BuildPeriody = jobSettingData.BuildPeriody,
                ScmSettings = scmList
            }, new JobConfiguration()
            {
                JobName = jobName,
                Configuration = Constants.DefaultConfiguration
            }))
            {
                Job job = jobsService.GetJob(jobName,fields);
                JobDto responseData = job.ToJobDto();
                if (realTime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.addJobCallBack(responseData);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.CreateActionFailed);
        }

       

        [Route("{jobName}")]
        [HttpDelete]
        public HttpResponseMessage deleteJob(string jobName,bool realTime=false)
        {
            if(jobsService.deleteJob(jobName))
            {
                if (realTime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.deleteJobCallBack(jobName);

                }
                return Request.CreateResponse(HttpStatusCode.OK, jobName);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.DeleteActionFailed);
        }
    }
}
