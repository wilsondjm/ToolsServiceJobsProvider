using ServiceProvider.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Service
{
    public class JobActionService
    {
        JobActionClient jobActionClient; 

        public JobActionService()
        {
            jobActionClient = new JobActionClient();
        }

        public bool startJob(string jobName)
        {
            return jobActionClient.startBuild(jobName);
        }

        public bool stopJob(string jobName)
        {
            return jobActionClient.stopaBuild(jobName);
        }
    }
}
