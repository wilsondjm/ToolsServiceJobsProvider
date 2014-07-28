using SDService.Model;
using SDService.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobInfoToData
    {
        public string jobName { set; get; }
        public JobSetting setting { set; get; }
        public JobConfiguration configuration { set; get; }
        public JobHistory builds { set; get; }
        public JobReport report { set; get; }
        public JobStatus status { set; get; }
    }
}