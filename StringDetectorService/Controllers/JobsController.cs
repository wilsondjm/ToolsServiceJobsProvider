using SDService.Model;
using SDService.Model.Basic;
using SDService.Model.Utils;
using ServiceLayer;
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

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobsController : ApiController
    {
        JobsService jobsService;

        public JobsController()
        {
            jobsService = new JobsService();
        }
        
        [Route("")]
        [HttpGet]
        public IEnumerable<JobInfoToData> getAllJobs()
        {
           Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);

            IEnumerable<Job> Jobs = jobsService.GetAllJobs(fields);
            IEnumerable<JobInfoToData> responseData =  Jobs.Select(job =>
                new JobInfoToData()
                {
                    jobName = job.JobName,
                    setting = job.JobSettings,
                    configuration = job.Configuration,
                    builds = job.Builds,
                    report = job.LastBuild,
                    status = job.Status,
                }
                );

            return responseData;
        }


        [Route("{jobName}")]
        [HttpGet]
        public JobInfoToData getJob(string jobName)
        {
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
            Job job = jobsService.GetJob(jobName,fields);
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

            return responseData;
        }

       

        [Route("{jobName}")]
        [HttpPost]
        public HttpResponseMessage createJob(string jobName, JobSettingToData jobSettingData)
        {
            Collection<string> fields = RequestFieldHelper.GetPartialResponseFields(Request);
            SCMSetting scmSetting = new SCMSetting()
            {
                SCMPort = jobSettingData.SCMPort,
                UserName = jobSettingData.UserName,
                Password = new PasswordEncryptionService().encryptString(jobSettingData.Passoword, HttpContext.Current.Server.MapPath(""), "\\..\\.."),
                Workspace = jobSettingData.Workspace,
                ViewMap = jobSettingData.ViewMap
            };
            var scmList = new List<SCMSetting>(); scmList.Add(scmSetting);

            if (jobsService.CreateJob(new JobSetting()
            {
                JobName = jobName,
                buildPeriody = jobSettingData.buildPeriody,
                scmSettings = scmList
            }, new JobConfiguration()
            {
                JobName = jobName,
                Configuration = Constants.DefaultConfiguration
            }))
            {
                Job job = jobsService.GetJob(jobName,fields);
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

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.CreateActionFailed);
        }

       

        [Route("{jobName}")]
        [HttpDelete]
        public HttpResponseMessage deleteJob(string jobName)
        {
            if(jobsService.deleteJob(jobName))
            {
                return Request.CreateResponse(HttpStatusCode.OK, jobName);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.DeleteActionFailed);
        }
    }
}
