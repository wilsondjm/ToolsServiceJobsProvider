using RequestClient;
using SDService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
   public  class JobSettingService
    {
        JobSettingClient jobSettingClient;
        public JobSettingService()
        {
            jobSettingClient = new JobSettingClient();
        }

        public bool updateJobSetting(JobSetting jobSetting)
        {
            return jobSettingClient.UpdateJobSetting(jobSetting);
        }

        public JobSetting getJobSetting(string jobName)
        {
            return jobSettingClient.QueryJobSetting(jobName);
        }
    }
}
