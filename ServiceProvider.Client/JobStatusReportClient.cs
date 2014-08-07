using ServiceProvider.Model;
using ServiceProvider.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Client
{
    public class JobReportsClient
    {
        //   lastBuild
       

        public JobReportsClient()
        {
           
        }

        public JobReport FetchReport(string projectName, string buildName = "lastBuild", string offSet = "0", string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/[BUILDNAME]/logText/progressiveText?start=[OFFSET]");
            JobReport queryProject = new JobReport() { JobName = projectName };
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", queryProject.JobName);
            baseURL.Replace("[BUILDNAME]", buildName);
            baseURL.Replace("[OFFSET]", offSet);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage response = client.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    System.Collections.Generic.IEnumerable<string> DataHeader;

                    string MoreData = response.Headers.TryGetValues("X-More-Data", out DataHeader) ? DataHeader.FirstOrDefault() : "false";
                    queryProject.Completed = MoreData.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? false : true;

                    string offset = response.Headers.GetValues("X-Text-Size").FirstOrDefault();
                    queryProject.Offset = Convert.ToInt32(offset);

                    queryProject.Report = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                }

                return queryProject;
            }
        }
    }
}
