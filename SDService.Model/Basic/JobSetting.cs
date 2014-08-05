using SDService.Model.Basic;
using SDService.Model.Utils;
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
        public SCMSetting ScmSetting {get;set;}
        public string BuildPeriody {get;set;}
    }

    public class JobSettingProperties
    {
        public bool containsJobName = false;
        public bool containsBuildPeriody = false;
        public bool containsScmSetting = false;
        public SCMSettingProperties scmSettingProperties;
        public JobSettingProperties(JobSetting setting)
        {
            setJobSettingFieldProperties(setting);
        }
        private void setJobSettingFieldProperties(JobSetting setting)
        {

            if (setting == null)
            {
                return;
            }

            if (setting.JobName != null)
            {
                containsJobName = true;
            }
            if (setting.BuildPeriody != null)
            {
                containsBuildPeriody = true;
            }
            if (setting.ScmSetting != null)
            {
                containsScmSetting = true;
                switch (Constants.ScmTypeDict[setting.ScmSetting.GetType()])
                {
                    case Constants.SCMType.GIT:
                        scmSettingProperties = new GitSettingProperties(setting.ScmSetting);
                        break;
                    case Constants.SCMType.SVN:
                        scmSettingProperties = new SVNSettingProperties(setting.ScmSetting);
                        break;
                    case Constants.SCMType.PERFORECE:
                        scmSettingProperties = new PerforceSettingProperties(setting.ScmSetting);
                        break;
                }

            }
        }
    }

    
}
