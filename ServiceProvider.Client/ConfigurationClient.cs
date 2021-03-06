﻿using ServiceProvider.Model;
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
    public class ConfigurationClient
    {
        public ConfigurationClient()
        {
        }

        public bool addConfiguration(string projectName, string config, string serverAddress = Constants.defaultServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/Configurations/";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync<JobConfiguration>(string.Empty, new JobConfiguration() { JobName = projectName, Configuration = config }).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsAsync<string>().Result;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool updateConfiguration(string projectName, string config, string serverAddress = Constants.defaultServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/Configurations/";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PutAsJsonAsync(string.Empty, new JobConfiguration() { JobName = projectName, Configuration = config }).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsAsync<string>().Result;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public JobConfiguration getConfiguration(string projectName, string serverAddress = Constants.defaultServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/Configurations/[PROJECTNAME]";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress).Replace("[PROJECTNAME]", projectName);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = client.GetAsync(projectName).Result;
                string responseContent = response.Content.ReadAsAsync<string>().Result;
                return  new JobConfiguration(){JobName = projectName,Configuration= responseContent};
            }
        }

        public bool deleConfiguration(string projectName, string serverAddress = Constants.defaultServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/Configurations/[PROJECTNAME]";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress).Replace("[PROJECTNAME]", projectName);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = client.DeleteAsync(projectName).Result;
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
