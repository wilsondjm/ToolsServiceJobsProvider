using SDService.Model;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RequestClient
{
    public  class ToolClient
    {
        public IEnumerable<Tool> QueryAllTools(string serverAddress = Constants.defaultJenkinsServerAddress)
        {
            string baseURL = "http://[SERVERADDRESS]/api/xml?tree=views[name]";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseToolsfromXml(xml);
            }
        }

        public Tool QueryTool(string toolName, string serverAddress = Constants.defaultJenkinsServerAddress,string prefix =Tool.Prifix)
        {
            string baseURL = "http://[SERVERADDRESS]/view/[PREFIX][TOOLNAME]/api/xml";
            string requestURL = baseURL.Replace("[SERVERADDRESS]", serverAddress).Replace("[PREFIX]", prefix).Replace("[TOOLNAME]",toolName);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestURL);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(string.Empty).Result;

                string xml = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                return JobConfigHelper.parseToolfromXml(xml, toolName);
            }
        }
    }
}
