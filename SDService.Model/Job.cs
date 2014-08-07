using ServiceProvider.Model.Basic;
using ServiceProvider.Model.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Model
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

    public class JobProperties
    {
        public bool containsJobName;
        public bool containsJobSetting;
        public bool containsJobConfig;
        public bool containsJobBuilds;
        public bool containsJobReport;
        public bool containsJobStatus;

        public JobProperties(Collection<string> fields)
        {
            setJobFieldProperties(fields);
        }
        private void setJobFieldProperties(Collection<string> fields)
        {

            if (fields == null || fields.Contains("*"))
            {
                containsJobName = true;
                containsJobSetting = true;
                //  containsJobConfig = true;
                containsJobBuilds = true;
                containsJobReport = true;
                containsJobStatus = true;
                return;
            }

            foreach (string field in fields)
            {
                string effectiveField = field.ToLower();
                switch (effectiveField)
                {
                    case Constants.JobNameField:
                        containsJobName = true;
                        break;
                    case Constants.JobSettingField:
                        containsJobSetting = true;
                        break;
                    case Constants.JobConfigField:
                        // containsJobConfig = true;
                        break;
                    case Constants.JobHistoryField:
                        containsJobBuilds = true;
                        break;
                    case Constants.JobStatusField:
                        containsJobStatus = true;
                        break;
                    case Constants.JobReportField:
                        containsJobReport = true;
                        break;
                }
            }
        }
    }
}
