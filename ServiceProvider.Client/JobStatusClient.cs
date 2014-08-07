using ServiceProvider.Model.Basic;
using ServiceProvider.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiceProvider.Client
{
   public class JobStatusClient
    {
          

         public JobStatusClient()
        { }

        public JobStatus getJobStatus(string jobName, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            string baseURLSTR = "http://[SERVERADDRESS]/job/[JOBNAME]/api/xml?tree=color";
            Dictionary<string, string> colorConvertMap = new Dictionary<string, string>(){
             {"red","Failed"},
             {"red_anime","InProgress"},
             {"yellow","Unstable"},
             {"yellow_anime","InProgress"},
             {"blue","Success"},
             {"blue_anime","InProgress"},
             {"grey","Pending"},
             {"grey_anime","InProgress"},
             {"disabled","Disabled"},
             {"disabled_anime","InProgress"},
             {"aborted","Aborted"},
             {"aborted_anime","InProgress"},
             {"notbuilt","NotBuilt"},
             {"notbuilt_anime","InProgress"},
            }; 
            StringBuilder baseURL = new StringBuilder(baseURLSTR);
            JobStatus status = new JobStatus();
            status.JobName = jobName;

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

                XDocument xDoc = XDocument.Parse(xml);
                status.Status = colorConvertMap[xDoc.Root.Element("color").Value];
               
            }
            return status;
        }
    }
}
