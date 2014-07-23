using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobInfoToData
    {
        public string jobName { set; get; }
       // public string buildPeriody { set; get; }
       // public string SCMPort { set; get; }
       // public string UserName { set; get; }
       // public string Passoword { set; get; }
       // public string Workspace { set; get; }
      //  public string ViewMap { set; get; }
        //public string Configuration { set; get; }
        public string lastBuildColor { set; get; }
        public string lastBuildStatus { set; get; }
    }
}