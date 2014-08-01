using SDService.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model
{

    public class Job
    {
        public string JobName { set; get; }
        public JobSetting Setting { set; get; }
        public JobConfiguration Configuration { set; get; }
        public JobReport Report { set; get; }
        public JobHistory Builds { set; get; }
        public JobStatus Status { get; set; }
    }
    
}
