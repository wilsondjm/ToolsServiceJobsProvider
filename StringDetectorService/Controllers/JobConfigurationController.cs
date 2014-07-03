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
    public class JobConfigurationController : ApiController
    {
        ConfigurationService configurationService;

        public JobConfigurationController()
        {
            configurationService = new ConfigurationService();
        }

        [Route("api/Configuration/{JobName}")]
        [HttpGet]
        public JobConfigurationToData getConfiguration(string JobName)
        {
            string result = configurationService.getConfiguration(JobName);
            return new JobConfigurationToData() { jobName = JobName, configuration = result};
        }

        [Route("api/Configuration/{JobName}")]
        [HttpPost]
        public IHttpActionResult AddConfiguration(string JobName, JobConfigurationToData data)
        {
            bool result = configurationService.addConfiguration(JobName, data.configuration);

            if (result)
                return Ok();
            return InternalServerError();
        }

        [Route("api/Configuration/{JobName}")]
        [HttpPut]
        public IHttpActionResult UpdateConfiguration(string JobName, JobConfigurationToData data)
        {
            bool result = configurationService.updateConfiguration(JobName, data.configuration);

            if (result)
                return Ok();
            return InternalServerError();
        }

        [Route("api/Configuration/{JobName}")]
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
