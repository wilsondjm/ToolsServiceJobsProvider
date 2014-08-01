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
        public IList<Job> QueryAllSDJobs(string serverAddress = Constants.defaultJenkinsServerAddress)
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

        public Job QueryJob(string jobName, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/job/[JOBNAME]/api/xml?tree=builds[duration,fullDisplayName,number,id,result],lastBuild[duration,fullDisplayName,number,id,result],color";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress).Replace("[JOBNAME]", jobName);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseJobFromXml(xml,jobName);
            }
        }



        public bool createJob(JobSetting jSetting, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            SCMSetting firstSetting = jSetting.ScmSettings.FirstOrDefault();

            string scmString = JobConfigHelper.getP4SingleDepotJobConfig(
                jSetting.JobName,
                firstSetting.UserName,
                firstSetting.Password,
                firstSetting.SCMPort,
                firstSetting.Workspace,
                firstSetting.ViewMap,
                jSetting.BuildPeriody
            );
            string baseURL = "http://[SERVERADDRESS]/createItem?name=[PROJECTNAME]";
            string requestURL = baseURL.Replace("[PROJECTNAME]", jSetting.JobName);
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

        public bool DeleteJob(string projectName, string serverAddress = Constants.defaultJenkinsServerAddress)
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

      

      
    }
}
