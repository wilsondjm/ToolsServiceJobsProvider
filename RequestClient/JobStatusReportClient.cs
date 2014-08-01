using SDService.Model;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestClient
{
    public class JobReportsClient
    {
        //   lastBuild
        StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/[BUILDNAME]/logText/progressiveText?start=[OFFSET]");

        public JobReportsClient(string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
        }

        public JobReport FetchReport(string projectName, string buildName = "lastBuild", string offSet = "0")
        {
            JobReport queryProject = new JobReport() { JobName = projectName };
            baseURL.Replace("[PROJECTNAME]", queryProject.JobName);
            baseURL.Replace("[BUILDNAME]", buildName);
            baseURL.Replace("[OFFSET]", offSet);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
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
