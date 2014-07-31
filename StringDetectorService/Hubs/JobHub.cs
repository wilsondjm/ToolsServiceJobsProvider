using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using SDService.Model;
using System.Text;
using System.Net.Http;
using System.Diagnostics;

namespace StringDetectorService.Hubs
{
    // This hub has no inbound APIs, since all inbound communication is done
    // via the HTTP API. It's here for clients which want to get continuous
    public class JobHub : Hub
    {
        public static ConcurrentDictionary<string, List<int>> _mapping = new ConcurrentDictionary<string, List<int>>();

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void fetchJobReport(string jobName)
        {
            StringBuilder baseURL = new StringBuilder("http://[SERVERADDRESS]/job/[PROJECTNAME]/lastBuild/logText/progressiveText?start=");
            string offset = "0";
            baseURL.Replace("[PROJECTNAME]", jobName);
            baseURL.Replace("[SERVERADDRESS]", "10.158.2.66:8080");
            string requestURL = baseURL.ToString();
            bool completed = false;
           
            while (!completed)
            {
                // wait for 2 seconds
                Stopwatch s = new Stopwatch();
                s.Start();
                while (s.Elapsed < TimeSpan.FromSeconds(2))
                {
                }
                s.Stop();
                using (var client = new HttpClient())
                {
               
                        client.BaseAddress = new Uri(requestURL+offset);
                        client.DefaultRequestHeaders.Accept.Clear();
                        HttpResponseMessage response = client.GetAsync("").Result;
                        if (response.IsSuccessStatusCode)
                        {
                            System.Collections.Generic.IEnumerable<string> DataHeader;

                            string MoreData = response.Headers.TryGetValues("X-More-Data", out DataHeader) ? DataHeader.FirstOrDefault() : "false";
                            completed = MoreData.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? false : true;

                            offset = response.Headers.GetValues("X-Text-Size").FirstOrDefault();

                            string report = System.Text.Encoding.UTF8.GetString((response.Content.ReadAsByteArrayAsync().Result));
                            Clients.All.appendReport(jobName,report);
                        }
                        //await 
                }
            }

             Clients.All.updateReportCallback(jobName);
        }
       
    }
}