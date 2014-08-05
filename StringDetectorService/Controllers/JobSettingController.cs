using Newtonsoft.Json;
using SDService.Model;
using SDService.Model.Basic;
using SDService.Model.Utils;
using ServiceLayer;
using StringDetectorService.Hubs;
using StringDetectorService.ReqResModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs/{jobName}/setting")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobSettingController : ApiControllerWithHub<JobHub>
    {
        JobSettingService jobSettingService;
        public JobSettingController()
        {
            jobSettingService = new JobSettingService();
        }

        [Route("")]
        [HttpGet]
        public JobSettingDto getJobSetting(string jobName)
        {
            JobSetting settings = jobSettingService.getJobSetting(jobName);
            return settings.ToJobSettingDto();;
        }

        [Route("")]
        [HttpPut]
        public HttpResponseMessage updateJobSetting(string jobName, JobSettingDto jobSettingData, bool realtime =false)
        {
            JobSetting jobSetting = jobSettingData.ToJobSetting();
            OperationResult<JobSetting> result =jobSettingService.updateJobSetting(jobSetting);
            if (result.IsSuccess)
            {
                JobSettingDto responseData = result.Entity.ToJobSettingDto();
                if (realtime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.updateJobSettingCallBack(responseData);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
           }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.UpdateSettingsFailed);
        }
       
    }
}
