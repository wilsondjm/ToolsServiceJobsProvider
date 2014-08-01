﻿using RequestClient;
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
        JobSettingClient settingClient;
        ConfigurationClient configClient;
        JobHistoryClient historyClient;
        JobReportsClient reportClient;
        JobStatusClient stausClient;
        public ToolService(){
            // init these clients
            toolClient = new ToolClient();
            settingClient = new JobSettingClient();
            configClient = new ConfigurationClient();
            historyClient = new JobHistoryClient();
            reportClient = new JobReportsClient();
            stausClient = new JobStatusClient();
        }


        public IEnumerable<Tool> GetAllTools(Collection<string> fields)
        {
            IList<Tool> tools = toolClient.QueryAllTools();
            IEnumerable<Tool> result= tools.Select(tool=> GetTool(tool.ToolName,fields)).ToList();

            return result;
        }

        public Tool GetTool(string toolName, Collection<string> fields)
        {
            Tool tool = toolClient.QueryTool(toolName);
            Dictionary<string,Collection<string>> fieldsMap= RequestFieldHelper.GetSecondLevelFields(fields);
            

            ToolFieldProperties toolFields = new ToolFieldProperties(fieldsMap==null?null:new List<string>(fieldsMap.Keys));
            if (toolFields.containsJobs)
            {
                JobFieldProperties jobFields = new JobFieldProperties(fieldsMap == null?null : fieldsMap[Constants.JobsField]);
                foreach (Job job in tool.Jobs)
                {
                    if (jobFields.containsJobSetting)
                    {
                        //Get JobSetting
                        job.Setting = settingClient.QueryJobSetting(job.JobName);
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
                        job.Report = reportClient.FetchReport(job.JobName);
                    }
                }

            }
            
            return tool;
        }
    }


    class ToolFieldProperties
    {
        public bool containsViewName = false;
        public bool containsToolName = false;
        public bool containsJobs = false;

        public ToolFieldProperties(ICollection<string> fields)
        {
            setToolFieldProperties(fields);
        }
        private void setToolFieldProperties(ICollection<string> fields)
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
