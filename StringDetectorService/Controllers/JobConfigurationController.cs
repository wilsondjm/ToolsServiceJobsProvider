using SDService.Model;
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
    [RoutePrefix("api/jobs/{jobName}/configuration")]
    public class JobConfigurationController : ApiController
    {
        ConfigurationService configurationService;

        public JobConfigurationController()
        {
            configurationService = new ConfigurationService();
        }

        [Route("")]
        [HttpGet]
        public JobConfigurationToData getConfiguration(string JobName)
        {
            JobConfiguration configuration = configurationService.getConfiguration(JobName);
            return new JobConfigurationToData() { jobName = configuration.JobName, configuration = configuration.Configuration };
        }

        [Route("")]
        [HttpPost]
        public HttpResponseMessage AddConfiguration(string JobName, JobConfigurationToData data)
        {
            bool result = configurationService.addConfiguration(JobName, data.configuration);

            if (result)
            {
                //JobConfiguration configuration = configurationService.getConfiguration(JobName);
                JobConfigurationToData responseData = new JobConfigurationToData() { jobName = JobName, configuration = data.configuration };
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.AddConfigurationFailed);
        }

        [Route("")]
        [HttpPut]
        public HttpResponseMessage UpdateConfiguration(string JobName, JobConfigurationToData data)
        {
            bool result = configurationService.updateConfiguration(JobName, data.configuration);

            if (result)
            {
                //JobConfiguration configuration = configurationService.getConfiguration(JobName);
                JobConfigurationToData responseData = new JobConfigurationToData() { jobName = JobName, configuration = data.configuration };
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.UpdateConfigurationFailed);
        }

        [Route("")]
        [HttpDelete]
        public HttpResponseMessage DeleteConfiguration(string JobName)
        {
            bool result = configurationService.deleteConfiguration(JobName);

            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, JobName);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.DeleteConfigurationFailed);
        }


    }
}
