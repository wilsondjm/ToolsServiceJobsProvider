using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobSettingToData
    {
        public string JobName { set; get; }
        public string buildPeriody { set; get; }
        public string SCMPort { set; get; }
        public string UserName { set; get; }
        public string Passoword { set; get; }
        public string Workspace { set; get; }
        public string ViewMap { set; get; }
    }
}