using SDService.Model.Basic;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RequestClient
{
    public class JobHistoryClient
    {
        
        public JobHistoryClient()
        { }

        public JobHistory getJobHistory(string jobName, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            string baseURLSTR = "http://[SERVERADDRESS]/job/[JOBNAME]/api/xml?tree=builds[duration,fullDisplayName,number,id,result],lastBuild[duration,fullDisplayName,number,id,result],color";
        
            StringBuilder baseURL = new StringBuilder(baseURLSTR);
            JobHistory history;

            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[JOBNAME]", jobName);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage response = client.GetAsync("").Result;
                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                history= JobConfigHelper.parseJobFromXml(xml, jobName).Builds;
            }
            return history;
        }
    }
}
