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
   public class JobSettingClient
    {

       public JobSetting QueryJobSetting(string jobName, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/config.xml");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", jobName);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
               // string upStreamProject = JobConfigHelper.parseUpStreamProjectfromXml(xml);
                return JobConfigHelper.parseCommonJobSettingsfromXml(xml, jobName);
            }
        }



       public bool UpdateJobSetting(JobSetting jSetting, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/config.xml");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", jSetting.JobName);
            string requestURL = baseURL.ToString();

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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

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
