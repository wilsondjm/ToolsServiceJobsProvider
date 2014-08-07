using ServiceProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobConfigDto
    {
        public string JobName { set; get; } //projectname
        public string Configuration { set; get; }
    }

    internal static class JobConfigurationExtension
    {
        internal static JobConfigDto ToJobConfigDto(this JobConfiguration configuration){
            if(configuration==null){
                return new JobConfigDto();
            }
            return new JobConfigDto(){
                JobName = configuration.JobName,
                Configuration = configuration.Configuration
            };
        }
    }

}