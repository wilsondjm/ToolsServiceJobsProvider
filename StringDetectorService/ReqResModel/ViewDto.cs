using SDService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class ViewDto
    {
        public string Name { get; set; }
        public string ViewName { get; set; }
        public ICollection<JobDto> Jobs { get; set; }
    }
    internal static class ViewExtension
    {
        internal static ViewDto ToViewDto(this View view)
        {
            if (view == null)
            {
                return new ViewDto();
            }
            ViewDto  viewDto= new ViewDto()
            {
                Name = view.Name,
                ViewName = view.ViewName,
                Jobs = view.Jobs.Select(job =>job.ToJobDto()).ToList()
            };
            return viewDto;
        }
    }
}