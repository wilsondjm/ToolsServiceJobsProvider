using Newtonsoft.Json;
using ServiceProvider.Model;
using ServiceProvider.Model.Basic;
using ServiceProvider.Model.Utils;
using ServiceProvider.Service;
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
        public HttpResponseMessage updateJobSetting(string jobName, JobSettingDto jobSettingData, bool realtime =false,string connectionId="")
        {
            JobSetting jobSetting = jobSettingData.ToJobSetting();
            OperationResult<JobSetting> result =jobSettingService.updateJobSetting(jobSetting);
            if (result.IsSuccess)
            {
                JobSettingDto responseData = result.Entity.ToJobSettingDto();
                if (realtime&&connectionId!="")
                {
                    // for the partial response reason ,some request will not require job name filed ,we will add it by hand.
                    responseData.JobName = jobName;
                    Hub.Clients.AllExcept(connectionId).updateJobSettingCallBack(responseData);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
           }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Constants.UpdateSettingsFailed);
        }
       
    }
}
