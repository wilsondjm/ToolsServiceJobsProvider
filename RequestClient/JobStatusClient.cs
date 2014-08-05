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
                HttpResponseMessage response = client.GetAsync("").Result;
                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));

                XDocument xDoc = XDocument.Parse(xml);
                status.Status = colorConvertMap[xDoc.Root.Element("color").Value];
               
            }
            return status;
        }
    }
}
