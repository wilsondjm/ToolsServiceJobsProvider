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
        public string ProjectName;
        public IEnumerable<SCMSetting> scmSettings;
        public string buildPeriody;
    }
}
