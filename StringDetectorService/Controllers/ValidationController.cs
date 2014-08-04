using SDService.Model.Utils;
using ServiceLayer;
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

        public ValidationController()
        {
            validationService = new JobValidationService();
        }

        [Route("api/validation/jobname/")]
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


        [Route("api/validation/timing/")]
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
