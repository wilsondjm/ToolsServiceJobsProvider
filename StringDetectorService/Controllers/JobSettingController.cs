using SDService.Model;
using SDService.Model.Basic;
using SDService.Model.Utils;
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
    [RoutePrefix("api/jobs/{jobName}/setting")]
    public class JobSettingController : ApiController
    {
        JobSettingService jobSettingService;
        public JobSettingController()
        {
            jobSettingService = new JobSettingService();
        }

        [Route("")]
        [HttpGet]
        public JobSettingToData getJobSetting(string jobName)
        {
            JobSetting settings = jobSettingService.getJobSetting(jobName);
            if (settings.scmSettings.Count() > 0)
            {
                return new JobSettingToData()
                {
                    JobName = settings.JobName,
                    buildPeriody = settings.buildPeriody,
                    SCMPort = settings.scmSettings.FirstOrDefault().SCMPort,
                    UserName = settings.scmSettings.FirstOrDefault().UserName,
                    Passoword = settings.scmSettings.FirstOrDefault().Password,
                    Workspace = settings.scmSettings.FirstOrDefault().Workspace,
                    ViewMap = settings.scmSettings.FirstOrDefault().ViewMap,
                };
            }
            return new JobSettingToData()
            {
                JobName = settings.JobName,
                buildPeriody = settings.buildPeriody,
            };
        }

        [Route("")]
        [HttpPut]
        public HttpResponseMessage updateJobSetting(string jobName, JobSettingToData jobSettingData)
        {
            SCMSetting scmSetting = new SCMSetting()
            {
                SCMPort = jobSettingData.SCMPort,
                UserName = jobSettingData.UserName,
                Password = jobSettingData.Passoword,
                Workspace = jobSettingData.Workspace,
                ViewMap = jobSettingData.ViewMap
            };
            var scmList = new List<SCMSetting>(); scmList.Add(scmSetting);
            JobSetting jobSetting = new JobSetting()
            {
                JobName = jobName,
                buildPeriody = jobSettingData.buildPeriody,
                scmSettings = scmList
            };

            if (jobSettingService.updateJobSetting(jobSetting))
            {
                JobSettingToData responseData = new JobSettingToData()
                {
                    JobName = jobSetting.JobName,
                    buildPeriody = jobSetting.buildPeriody,
                    SCMPort = jobSetting.scmSettings.FirstOrDefault().SCMPort,
                    UserName = jobSetting.scmSettings.FirstOrDefault().UserName,
                    Passoword = jobSetting.scmSettings.FirstOrDefault().Password,
                    Workspace = jobSetting.scmSettings.FirstOrDefault().Workspace,
                    ViewMap = jobSetting.scmSettings.FirstOrDefault().ViewMap,
                };
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
           }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.UpdateSettingsFailed);
        }
    }
}
