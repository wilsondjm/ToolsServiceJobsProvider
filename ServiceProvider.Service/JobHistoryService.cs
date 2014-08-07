using ServiceProvider.Client;
using ServiceProvider.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Service
{
    public class JobHistoryService
    {
        JobHistoryClient historyClient;

        public JobHistoryService()
        {
            historyClient = new JobHistoryClient();
        }

        public JobHistory getAllJobHistory(string jobName)
        {
            return historyClient.getJobHistory(jobName);
        }

    }
}
