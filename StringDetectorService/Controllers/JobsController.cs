using SDService.Model;
using SDService.Model.Basic;
using ServiceLayer;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
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
            IEnumerable<Job> Jobs = jobsService.GetAllJobs();
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
            Job job = jobsService.GetJob(jobName);
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
        public IHttpActionResult createJob(string jobName, JobSettingToData jobSettingData)
        {
            
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
                Configuration = jobSettingData.Configuration
            }))
            {
                return this.Ok();
            }
            return BadRequest("Failed to create the job.");
        }

       

        [Route("{jobName}")]
        [HttpDelete]
        public IHttpActionResult deleteJob(string jobName)
        {
            if(jobsService.deleteJob(jobName))
            {
                return Ok("Job Deleted");
            }
            return BadRequest();
        }
    }
}
