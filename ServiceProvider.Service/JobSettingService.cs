using ServiceProvider.Client;
using ServiceProvider.Model;
using ServiceProvider.Model.Basic;
using ServiceProvider.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Service
{
   public  class JobSettingService
    {
        JobSettingClient jobSettingClient;
        public JobSettingService()
        {
            jobSettingClient = new JobSettingClient();
        }

        public OperationResult<JobSetting> updateJobSetting(JobSetting jobSetting)
        {
            return jobSettingClient.UpdateJobSetting(jobSetting,new JobSettingProperties(jobSetting));
        }

        public JobSetting getJobSetting(string jobName)
        {
            return jobSettingClient.QueryJobSetting(jobName);
        }

    }

  
}
