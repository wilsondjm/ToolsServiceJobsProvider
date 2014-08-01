using SDService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobReportDto
    {
        public string JobName { set; get; }
        public bool Completed { set; get; }
        public string Report { set; get; }
        public int Offset { set; get; }
    }

    internal static class JobReportExtension
    {
        internal static JobReportDto ToJobReportDto(this JobReport report)
        {
            if (report == null)
            {
                return new JobReportDto();
            }
            return new JobReportDto()
            {
                JobName = report.JobName,
                Completed = report.Completed,
                Report = report.Report,
                Offset = report.Offset
            };
        }
    }
}