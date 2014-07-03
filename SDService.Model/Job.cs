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
        public JobSetting JobSettings { set; get; }
        public JobConfiguration Configuration { set; get; }
        public JobReport LastBuild { set; get; }
        public string color;
    }
}
