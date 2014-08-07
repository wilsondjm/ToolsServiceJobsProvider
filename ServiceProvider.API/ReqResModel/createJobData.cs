using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class CreateJobData
    {
        public string JobName { get; set; }
        public string UpstreamProject { get; set; }
    }
}