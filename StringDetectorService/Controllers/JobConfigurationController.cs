﻿using SDService.Model;
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

namespace StringDetectorService.Controllers
{
    [RoutePrefix("api/jobs/{jobName}/configuration")]
    public class JobConfigurationController : ApiControllerWithHub<JobHub>
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
        public HttpResponseMessage AddConfiguration(string JobName, JobConfigurationToData data, bool realtime)
        {
            bool result = configurationService.addConfiguration(JobName, data.configuration);

            if (result)
            {
                //JobConfiguration configuration = configurationService.getConfiguration(JobName);
                JobConfigurationToData responseData = new JobConfigurationToData() { jobName = JobName, configuration = data.configuration };
                if (realtime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.addConfigurationCallBack(responseData);
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.AddConfigurationFailed);
        }

        [Route("")]
        [HttpPut]
        public HttpResponseMessage UpdateConfiguration(string JobName, JobConfigurationToData data,bool realtime =false)
        {
            bool result = configurationService.updateConfiguration(JobName, data.configuration);

            if (result)
            {
                //JobConfiguration configuration = configurationService.getConfiguration(JobName);
                JobConfigurationToData responseData = new JobConfigurationToData() { jobName = JobName, configuration = data.configuration };
                if (realtime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.updateConfigurationCallBack(responseData);
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.UpdateConfigurationFailed);
        }

        [Route("")]
        [HttpDelete]
        public HttpResponseMessage DeleteConfiguration(string JobName,bool realtime=false)
        {
            bool result = configurationService.deleteConfiguration(JobName);

            if (result)
            {
                if (realtime)
                {
                    // we will try to set partital response for real time next version
                    Hub.Clients.All.deleteConfigurationCallBack(JobName);
                }
                return Request.CreateResponse(HttpStatusCode.OK, JobName);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, Constants.DeleteConfigurationFailed);
        }


    }
}
