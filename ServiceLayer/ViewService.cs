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
    public class ViewService
    {
        // tool service use some clients 
        ViewClient viewClient;
        JobSettingClient settingClient;
        ConfigurationClient configClient;
        JobHistoryClient historyClient;
        JobReportsClient reportClient;
        JobStatusClient stausClient;
        public ViewService(){
            // init these clients
            viewClient = new ViewClient();
            settingClient = new JobSettingClient();
            configClient = new ConfigurationClient();
            historyClient = new JobHistoryClient();
            reportClient = new JobReportsClient();
            stausClient = new JobStatusClient();
        }


        public IEnumerable<View> GetAllViews(Collection<string> fields,string categoryTag)
        {
            IList<View> views = viewClient.QueryAllViews(categoryTag);
            IEnumerable<View> result = views.Select(view => GetView(view.Name, fields, categoryTag)).ToList();

            return result;
        }

        public View GetView(string name, Collection<string> fields, string categoryTag)
        {
            View view = viewClient.QueryView(name,categoryTag);
            Dictionary<string,Collection<string>> fieldsMap= RequestFieldHelper.GetSecondLevelFields(fields);
            

            ViewFieldProperties viewFields = new ViewFieldProperties(fieldsMap==null?null:new List<string>(fieldsMap.Keys));
            if (viewFields.containsJobs)
            {
                JobProperties jobFields = new JobProperties(fieldsMap == null?null : fieldsMap[Constants.JobsField]);
                foreach (Job job in view.Jobs)
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
            
            return view;
        }
    }


   
}
