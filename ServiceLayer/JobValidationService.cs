using RequestClient;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class JobValidationService
    {
        JobValidationClient validationClient;

        public JobValidationService()
        {
            validationClient = new JobValidationClient();
        }

        public OperationResult validatJobName(string jobName)
        {
            return validationClient.validateJobName(jobName);
        }

        public OperationResult validateTiming(string jobName)
        {
            return validationClient.validateTimeSetting(jobName);
        }
    }
}
