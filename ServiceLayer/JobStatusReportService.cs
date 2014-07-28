using RequestClient;
using SDService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class JobStatusReportService
    {
        JobReportsClient jobsSRClient;

        public JobStatusReportService()
        {
            jobsSRClient = new JobReportsClient();// there is a default serverAddress
        }

        public JobReport getReport(string jobName, string buildName = "lastBuild", string offSet = "0")
        {
            return jobsSRClient.FetchReport(jobName, buildName, offSet);
        }



    }
}
