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
    public class JobsClient
    {
        public IEnumerable<Job> QueryAllSDJobs(string serverAddress = "10.158.2.66:8080")
        {
            string baseURL = "http://[SERVERADDRESS]/api/xml";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseJobsfromXml(xml);
            }
        }

        public bool createJob(JobSetting jSetting, string serverAddress = "10.158.2.66:8080")
        {
            SCMSetting firstSetting = jSetting.scmSettings.FirstOrDefault();

            string scmString = JobConfigHelper.getP4SingleDepotJobConfig(
                jSetting.ProjectName,
                firstSetting.UserName,
                firstSetting.Passoword,
                firstSetting.SCMPort,
                firstSetting.Workspace,
                firstSetting.ViewMap,
                jSetting.buildPeriody
            );
            string baseURL = "http://[SERVERADDRESS]/createItem?name=[PROJECTNAME]";
            string requestURL = baseURL.Replace("[PROJECTNAME]", jSetting.ProjectName);
            requestURL = requestURL.Replace("[SERVERADDRESS]", serverAddress);

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

        public bool DeleteJob(string projectName, string serverAddress = "10.158.2.66:8080")
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/doDelete");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", projectName);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.PostAsync("", new StringContent(String.Empty)).Result;

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

        public bool UpdateJob(JobSetting jSetting)
        {
            string baseURL = "http://10.158.2.66:8080/job/[PROJECTNAME]/config.xml";
            string requestURL = baseURL.Replace("[PROJECTNAME]", jSetting.ProjectName);

            SCMSetting firstSetting = jSetting.scmSettings.FirstOrDefault();
            string scmString = JobConfigHelper.getP4SingleDepotJobConfig(
                jSetting.ProjectName,
                firstSetting.UserName,
                firstSetting.Passoword,
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

        public JobSetting QueryJobSetting(string projectName, string serverAddress = "10.158.2.66:8080")
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/config.xml");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", projectName);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseJobSettingsfromXml(xml, projectName);
            }
        }
    }
}
