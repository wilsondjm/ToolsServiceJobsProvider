using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobReportToData
    {
        public string JobName { set; get; }
        public string Completed { set; get; }
        public string Report { set; get; }
        public string Offset { set; get; }
    }
}