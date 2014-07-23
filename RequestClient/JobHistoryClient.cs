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
            JobHistory history = new JobHistory();
            history.jobName = jobName;

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
                history.colorStatus = xDoc.Root.Element("color").Value;
                if (history.colorStatus.Equals("notbuilt", StringComparison.InvariantCultureIgnoreCase))
                {
                    //no build history for this project
                    return history;
                }

                history.jobHistories = xDoc.Descendants("build").Select(
                    build => new HistoryItem()
                    {
                        duration = build.Element("duration").Value,
                        fullDisplayName = build.Element("fullDisplayName").Value,
                        id = build.Element("id").Value,
                        number = build.Element("number").Value,
                        result = build.Element("result")==null?"Pending":build.Element("result").Value
                    }
                ).ToList<HistoryItem>();
                XElement lastBuildElement = xDoc.Root.Element("lastBuild");
                history.lastBuild = new HistoryItem()
                {
                    duration = lastBuildElement.Element("duration").Value,
                    fullDisplayName = lastBuildElement.Element("fullDisplayName").Value,
                    id = lastBuildElement.Element("id").Value,
                    number = lastBuildElement.Element("number").Value,
                    result = lastBuildElement.Element("result") == null ? "Pending" : lastBuildElement.Element("result").Value
                };
            }
            return history;
        }
    }
}
