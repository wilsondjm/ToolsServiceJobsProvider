using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestClient
{
    public class JobActionClient
    {
        
        public bool startBuild(string projectName, string serverAddress = "10.158.2.66:8080")
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/build?delay=0sec");
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

        public bool stopaBuild(string projectName, string serverAddress = "10.158.2.66:8080", string buildNumber = "lastBuild")
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/[BUILDNUMBER]/stop");
            baseURL.Replace("[SERVERADDRESS]", serverAddress);
            baseURL.Replace("[PROJECTNAME]", projectName);
            baseURL.Replace("[BUILDNUMBER]", buildNumber);
            string requestURL = baseURL.ToString();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = client.PostAsync("", new StringContent(String.Empty)).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.Found)
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
