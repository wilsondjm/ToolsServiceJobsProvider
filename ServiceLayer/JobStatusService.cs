using ServiceProvider.Client;
using ServiceProvider.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Service
{
    public class JobStatusService
    {
         JobStatusClient statusClient;

         public JobStatusService()
        {
            statusClient = new JobStatusClient();
        }

        public JobStatus getJobStatus(string jobName)
        {
            return statusClient.getJobStatus(jobName);
        }
    }
}
