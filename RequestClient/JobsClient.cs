using SDService.Model;
using SDService.Model.Basic;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

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
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseJobFromXml(xml,jobName);
            }
        }



        public bool createJob(string jobName,string upstreamProject, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
           
            string baseURL = "http://[SERVERADDRESS]/createItem?name=[PROJECTNAME]";
            string requestURL = baseURL.Replace("[PROJECTNAME]", jobName);
            requestURL = requestURL.Replace("[SERVERADDRESS]", serverAddress);
            string toolName = jobName.Split('-').Last();
            string assignNode=Constants.defaultAssignNode;
            using (var client = new HttpClient())
            {
                string fetchUrl="http://[SERVERADDRESS]/job/[PROJECTNAME]/config.xml".Replace("[SERVERADDRESS]", serverAddress).Replace("[PROJECTNAME]", upstreamProject);
                client.BaseAddress = new Uri(fetchUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                string  node=JobConfigHelper.getAssignNode(xml);
                if(node!=null){
                    assignNode = node;
                }
            }

            string configXml = JobConfigHelper.getTemplateConfigXml(toolName,upstreamProject,assignNode);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpRequestMessage request = new HttpRequestMessage();
                request.Content = new StringContent(configXml);
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
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

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
