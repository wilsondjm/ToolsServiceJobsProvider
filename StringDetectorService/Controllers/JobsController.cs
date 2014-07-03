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
    [RoutePrefix("api/Jobs")]
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
                    //buildPeriody = job.JobSettings.buildPeriody,
                    //SCMPort = job.JobSettings.scmSettings.FirstOrDefault().SCMPort,
                    //UserName = job.JobSettings.scmSettings.FirstOrDefault().UserName,
                    //Passoword =job.JobSettings.scmSettings.FirstOrDefault().Passoword,
                    //Workspace = job.JobSettings.scmSettings.FirstOrDefault().Workspace,
                    //ViewMap = job.JobSettings.scmSettings.FirstOrDefault().ViewMap,
                    //Configuration = Thread.CurrentPrincipal.ToString(),
                    lastBuildColor = job.color,
                    lastBuildStatus = job.LastBuild.Completed.ToString()
                }
                );

            return responseData;
        }

        [Route("{jobName}")]
        [HttpGet]
        public JobSettingToData getJobSetting(string jobName)
        {
            JobSetting settings = jobsService.readJobConfig(jobName);
            return new JobSettingToData()
            {
                JobName = settings.ProjectName,
                buildPeriody = settings.buildPeriody,
                SCMPort = settings.scmSettings.FirstOrDefault().SCMPort,
                UserName = settings.scmSettings.FirstOrDefault().UserName,
                Passoword = settings.scmSettings.FirstOrDefault().Passoword,
                Workspace = settings.scmSettings.FirstOrDefault().Workspace,
                ViewMap = settings.scmSettings.FirstOrDefault().ViewMap,
                Configuration = ""
            };
        }

        [Route("{jobName}")]
        [HttpPost]
        public IHttpActionResult createJob(string jobName, JobSettingToData jobSettingData)
        {
            
            SCMSetting scmSetting = new SCMSetting()
            {
                SCMPort = jobSettingData.SCMPort,
                UserName = jobSettingData.UserName,
                Passoword = new PasswordEncryptionService().encryptString(jobSettingData.Passoword, HttpContext.Current.Server.MapPath(""), "\\..\\.."),
                Workspace = jobSettingData.Workspace,
                ViewMap = jobSettingData.ViewMap
            };
            var scmList = new List<SCMSetting>(); scmList.Add(scmSetting);

            if (jobsService.CreateJob(new JobSetting()
            {
                ProjectName = jobName,
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
        [HttpPut]
        public IHttpActionResult updateJobSetting(string jobName, JobSettingToData jobSettingData)
        {
            SCMSetting scmSetting = new SCMSetting()
            {
                SCMPort = jobSettingData.SCMPort,
                UserName = jobSettingData.UserName,
                Passoword = jobSettingData.Passoword,
                Workspace = jobSettingData.Workspace,
                ViewMap = jobSettingData.ViewMap
            };
            var scmList = new List<SCMSetting>(); scmList.Add(scmSetting);

            if (jobsService.updateJobConfig(new JobSetting()
            {
                ProjectName = jobName,
                buildPeriody = jobSettingData.buildPeriody,
                scmSettings = scmList
            }))
                return Ok();
            return BadRequest();
        }

        [Route("{jobName}")]
        [HttpDelete]
        public IHttpActionResult deleteJob(string jobName)
        {
            if(jobsService.deleteJob(jobName))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
