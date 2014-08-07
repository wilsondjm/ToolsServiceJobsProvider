using ServiceProvider.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobStatusDto
    {
        public string JobName { set; get; }
        public string Status { set; get; }
    }

    internal static class JobStatusExtension
    {
        internal static JobStatusDto ToJobStatusDto(this JobStatus status)
        {
            if (status == null)
            {
                return new JobStatusDto();
            }
            return new JobStatusDto()
            {
                JobName = status.JobName,
                Status = status.Status
            };
        }
    }
}