using RequestClient;
using SDService.Model;
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
        ViewClient viewClient;

        public JobValidationService()
        {
            validationClient = new JobValidationClient();
            viewClient = new ViewClient();
        }

        public OperationResult validatJobName(string jobName)
        {
            return validationClient.validateJobName(jobName);
        }

        public OperationResult validateTiming(string jobName)
        {
            return validationClient.validateTimeSetting(jobName);
        }

        public OperationResult validateCustomJobName(string jobName)
        {
            string[] array = jobName.Split('-');
            string projectName = array.First();
            string toolName = array.Last();
            bool containsProject=false;
            bool containsTool=false;
            IEnumerable<string> viewNames= viewClient.QueryAllViews().Select(view => view.ViewName);
            foreach (string viewName in viewNames)
            {
                if(containsProject&& containsTool){
                    break;
                }

                if (viewName == Constants.ProjectTag + projectName)
                {
                    containsProject = true;
                }
                else if (viewName == Constants.ToolTag + toolName)
                {
                    containsTool = true;
                }
            }
            return new OperationResult(containsTool && containsProject);
        }
    }
}
