using SDService.Model;
using SDService.Model.Basic;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestClient
{
   public class JobSettingClient
    {

        public JobSetting QueryJobSetting(string jobName, string serverAddress = "10.158.2.66:8080")
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/config.xml");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", jobName);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseJobSettingsfromXml(xml, jobName);
            }
        }

        public bool UpdateJobSetting(JobSetting jSetting)
        {
            string baseURL = "http://10.158.2.66:8080/job/[PROJECTNAME]/config.xml";
            string requestURL = baseURL.Replace("[PROJECTNAME]", jSetting.JobName);

            SCMSetting firstSetting = jSetting.scmSettings.FirstOrDefault();
            string scmString = JobConfigHelper.getP4SingleDepotJobConfig(
                jSetting.JobName,
                firstSetting.UserName,
                firstSetting.Password,
                firstSetting.SCMPort,
                firstSetting.Workspace,
                firstSetting.ViewMap,
                jSetting.buildPeriody
            );

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpRequestMessage request = new HttpRequestMessage();
                request.Content = new StringContent(scmString);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                request.RequestUri = new Uri(requestURL);
                request.Method = HttpMethod.Post;
                HttpResponseMessage response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
