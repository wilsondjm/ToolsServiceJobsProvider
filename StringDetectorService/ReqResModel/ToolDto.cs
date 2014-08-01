using SDService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class ToolDto
    {
        public string ToolName { get; set; }
        public string ViewName { get; set; }
        public ICollection<JobDto> Jobs { get; set; }
    }
    internal static class ToolExtension
    {
        internal static ToolDto ToToolDto(this Tool tool)
        {
            if (tool == null)
            {
                return new ToolDto();
            }
            ToolDto  toolDto= new ToolDto()
            {
                ToolName = tool.ToolName,
                ViewName = tool.ViewName,
                Jobs = tool.Jobs.Select(job =>job.ToJobDto()).ToList()
            };
            return toolDto;
        }
    }
}