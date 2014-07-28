using SDService.Model;
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
        public IHttpActionResult AddConfiguration(string JobName, JobConfigurationToData data)
        {
            bool result = configurationService.addConfiguration(JobName, data.configuration);

            if (result)
                return Ok();
            return InternalServerError();
        }

        [Route("")]
        [HttpPut]
        public IHttpActionResult UpdateConfiguration(string JobName, JobConfigurationToData data)
        {
            bool result = configurationService.updateConfiguration(JobName, data.configuration);

            if (result)
                return Ok();
            return InternalServerError();
        }

        [Route("")]
        [HttpDelete]
        public IHttpActionResult DeleteConfiguration(string JobName)
        {
            bool result = configurationService.deleteConfiguration(JobName);

            if (result)
                return Ok();
            return InternalServerError();
        }


    }
}
