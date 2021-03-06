﻿using ServiceProvider.Model;
using ServiceProvider.Model.Basic;
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
                if (xml.Contains(Constants.UpstreamProjectFlag))
                {
                   string upStreamProject = JobConfigHelper.parseUpStreamProjectfromXml(xml);

                   return QueryJobSetting( upStreamProject);
                }
               
                return JobConfigHelper.parseCommonJobSettingsfromXml(xml, jobName);
            }
        }




       public OperationResult<JobSetting> UpdateJobSetting(JobSetting jSetting, JobSettingProperties properties, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/config.xml");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", jSetting.JobName);
            string requestURL = baseURL.ToString();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage getResponse = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((getResponse.Content.ReadAsByteArrayAsync().Result));
                if (xml.Contains(Constants.UpstreamProjectFlag))
                {
                    string upStreamProject = JobConfigHelper.parseUpStreamProjectfromXml(xml);
                    jSetting.JobName = upStreamProject;
                    // update the upStreamProject setting
                    return UpdateJobSetting(jSetting,properties);
                }

               // return JobConfigHelper.parseCommonJobSettingsfromXml(xml, jobName);
                xml = JobConfigHelper.updateJobSetting(xml, jSetting,properties);
                HttpRequestMessage request = new HttpRequestMessage();
                request.Content = new StringContent(xml);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                request.RequestUri = new Uri(requestURL);
                request.Method = HttpMethod.Post;
                HttpResponseMessage postResponse = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;

                if (postResponse.IsSuccessStatusCode)
                {
                    return new OperationResult<JobSetting>(true) {  Entity= JobConfigHelper.parseCommonJobSettingsfromXml(xml,jSetting.JobName)};
                }
                else
                {
                    return new OperationResult<JobSetting>(false) { };
                }

            }
        }

    }

   
}
