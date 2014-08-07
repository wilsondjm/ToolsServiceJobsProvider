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
    public class JobValidationClient
    {
        public JobValidationClient()
        {
        }

        public OperationResult validateJobName(string jobName, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/checkJobName?value=";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress)+jobName;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    if(responseContent.Contains("class=")) {
                        string[] splitArray = responseContent.Split(new Char[] { '<', '>' },
                                 StringSplitOptions.RemoveEmptyEntries);
                        string type = splitArray.First().Split(new String[] { "class=" }, StringSplitOptions.None).Last();
                        string description = splitArray[2];

                        return new OperationResult(true) { Type=type ,Message=description };
                        
                    }
                    else {
                        return new OperationResult(true) {Type="ok" ,Message="The name is unique"};
                    }
                }
                else
                {
                    return new OperationResult(false) { };
                }
            }
        }

        public OperationResult validateTimeSetting(string timingStr, string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/job/validation/descriptorByName/hudson.triggers.TimerTrigger/checkSpec?value=";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress)+timingStr;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();
                // authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", Constants.JenkinsUserName, Constants.JenkinsPassword))));
                HttpResponseMessage response = client.GetAsync(string.Empty).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    if (responseContent.Contains("class="))
                    {
                        string[] splitArray = responseContent.Split(new Char[] { '<', '>' },
                                 StringSplitOptions.RemoveEmptyEntries);
                        string type = splitArray.First().Split(new String[] { "class=" }, StringSplitOptions.None).Last();
                        string description = splitArray[2];
                        return new OperationResult(true) { Type = type, Message = description };
                    }
                    else
                    {
                        return new OperationResult(true) { Type = "ok", Message = "The setting is ok" };
                    }
                }
                else
                {
                    return new OperationResult(false) { };
                }
            }
        }
       
    }
}
