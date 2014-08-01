using RequestClient;
using SDService.Model;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ToolService
    {
        // tool service use some clients 
        ToolClient toolClient;

        public ToolService(){
            // init these clients
            toolClient = new ToolClient();
        }


        public IEnumerable<Tool> GetAllTools(Collection<string> fields)
        {
            IEnumerable<Tool> tools = toolClient.QueryAllTools();
            foreach (Tool tool in tools)
            {

            }

            return null;
        }

        public Tool GetTool(string toolName, Collection<string> fields)
        {
            Tool tool = toolClient.QueryTool(toolName);



            JobFieldProperties jobFields = new JobFieldProperties(fields);
            foreach (Job job in tool.Jobs)
            {

                if (jobFields.containsJobSetting)
                {
                    //Get JobSetting
                    job.JobSettings = settingClient.QueryJobSetting(job.JobName);
                }
                if (jobFields.containsJobConfig)
                {
                    //Get JobConfiguration
                    job.Configuration = configClient.getConfiguration(job.JobName);
                }
                if (jobFields.containsJobBuilds)
                {
                    //Get Job Build History
                    job.Builds = historyClient.getJobHistory(job.JobName);
                }
                if (jobFields.containsJobReport)
                {
                    //Get Job LastBuild
                    job.LastBuild = reportClient.FetchReport(job.JobName);
                }
            }

            return null;
        }
    }


    class ToolFieldProperties
    {
        public bool containsViewName = false;
        public bool containsToolName = false;
        public bool containsJobs = false;

        public ToolFieldProperties(Collection<string> fields)
        {
            setToolFieldProperties(fields);
        }
        private void setToolFieldProperties(Collection<string> fields)
        {

            if (fields == null || fields.Contains("*"))
            {
                containsViewName = true;
                containsToolName = true;
                containsJobs = true;
                return;
            }

            foreach (string field in fields)
            {
                string effectiveField = field.ToLower();
                switch (effectiveField)
                {
                    case Constants.ViewNameField:
                        containsViewName = true;
                        break;
                    case Constants.ToolNameField:
                        containsToolName = true;
                        break;
                    case Constants.JobsField:
                        containsJobs = true;
                        break;
                }
            }
        }
    }
}
