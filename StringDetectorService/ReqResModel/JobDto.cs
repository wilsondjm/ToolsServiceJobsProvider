using ServiceProvider.Model;
using ServiceProvider.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobDto
    {
        public string JobName { set; get; }
        public JobSettingDto Setting { set; get; }
        public JobConfigDto Configuration { set; get; }
        public JobHistoryDto Builds { set; get; }
        public JobReportDto Report { set; get; }
        public JobStatusDto Status { set; get; }
    }
    internal static class JobExtension
    {
        internal static JobDto ToJobDto(this Job job)
        {
            if (job == null)
            {
                return new JobDto();
            }
            return new JobDto()
            {
                JobName = job.JobName,
                Setting = job.Setting.ToJobSettingDto(),
                Configuration = job.Configuration.ToJobConfigDto(),
                Builds = job.Builds.ToJobHistoryDto(),
                Report = job.Report.ToJobReportDto(),
                Status = job.Status.ToJobStatusDto()
            };
        }
    }
}