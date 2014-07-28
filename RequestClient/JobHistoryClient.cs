using SDService.Model.Basic;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RequestClient
{
    public class JobHistoryClient
    {
        private string baseURLSTR = "http://[SERVERADDRESS]/job/[JOBNAME]/api/xml?tree=builds[duration,fullDisplayName,number,id,result],lastBuild[duration,fullDisplayName,number,id,result],color";
        
        public JobHistoryClient()
        { }

        public JobHistory getJobHistory(string jobName, string serverAddress = "10.158.2.66:8080")
        {
            StringBuilder baseURL = new StringBuilder(baseURLSTR);
            JobHistory history;

            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[JOBNAME]", jobName);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = client.GetAsync("").Result;
                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                history= JobConfigHelper.parseJobFromXml(xml, jobName).Builds;
            }
            return history;
        }
    }
}
