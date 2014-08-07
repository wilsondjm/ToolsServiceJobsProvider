using ServiceProvider.Model.Utils;
using ServiceProvider.Service;
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
     [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValidationController : ApiController
    {
        private JobValidationService validationService;
        private ViewService viewService;

        public ValidationController()
        {
            validationService = new JobValidationService();
            viewService = new ViewService();
        }

         // the validation is defined by jenkins
        [Route("api/validation/jenkins/jobname")]
        [HttpPost]
        public HttpResponseMessage validateJobName(ValidationData data)
        {
            string jobName = System.Web.HttpUtility.HtmlEncode(data.Input);
            OperationResult  result= validationService.validatJobName(jobName);
            if (result.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK,result);
            }
            else {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            
        }

         // the valid is defined by self
        [Route("api/validation/custom/jobname")]
        [HttpPost]
        public HttpResponseMessage validateCustomJobName(ValidationData data)
        {
            string jobName = System.Web.HttpUtility.HtmlEncode(data.Input);

            OperationResult result = validationService.validateCustomJobName(jobName);
            if (result.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }


        [Route("api/validation/jenkins/timing")]
        [HttpPost]
        public HttpResponseMessage validateTiming(ValidationData data)
        {
             string timing = System.Web.HttpUtility.HtmlEncode(data.Input);
            OperationResult result = validationService.validateTiming(timing);
            if (result.IsSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

    }
}
