using SDService.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model
{
    public class JobSetting
    {
        public string JobName { get; set; }
        public IEnumerable<SCMSetting> scmSettings {get;set;}
        public string buildPeriody {get;set;}
    }
}
