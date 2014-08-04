using SDService.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobHistoryDto
    {
        public string JobName { set; get; }
        public IEnumerable<HistoryItemDto> JobHistories { set; get; }
        public HistoryItemDto LastBuild { set; get; }
       
    }
    public class HistoryItemDto
    {
        public string Duration { set; get; }
        public string FullDisplayName { set; get; }
        public string Id { set; get; }
        public string Number { set; get; }
        public string Result { set; get; }
    }

    internal static class JobHistoryExtension
    {
        internal static JobHistoryDto ToJobHistoryDto(this JobHistory builds)
        {
            if (builds == null)
            {
                return new JobHistoryDto();
            }
            return new JobHistoryDto()
            {
                JobName = builds.JobName,
                JobHistories =builds.JobHistories.Select(item => item.ToHistoryItemDto()).ToList(),
                LastBuild = builds.LastBuild.ToHistoryItemDto()
            };

        }
        internal static HistoryItemDto ToHistoryItemDto(this HistoryItem item)
        {
            if (item == null)
            {
                return new HistoryItemDto();
            }
            return new HistoryItemDto()
            {
                Duration = item.Duration,
                FullDisplayName = item.FullDisplayName,
                Id = item.Id,
                Number = item.Number,
                Result = item.Result
            };
        }
    }
}