using SDService.Model;
using SDService.Model.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobSettingDto
    {
        public string JobName { set; get; }
        public string BuildPeriody { set; get; }
        public string SCMPort { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Workspace { set; get; }
        public string ViewMap { set; get; }
    }

    internal static class JobSettingExtension
    {
        internal static JobSettingDto ToJobSettingDto(this JobSetting jobSetting)
        {
            if (jobSetting == null)
            {
                return new JobSettingDto();
            }
            if (jobSetting.scmSettings == null || jobSetting.scmSettings.Count() == 0)
            {
                return new JobSettingDto()
                {

                    JobName = jobSetting.JobName,
                    BuildPeriody = jobSetting.buildPeriody,
                };
            }
            return new JobSettingDto
            {
                JobName = jobSetting.JobName,
                BuildPeriody = jobSetting.buildPeriody,
                SCMPort = jobSetting.scmSettings.FirstOrDefault().SCMPort,
                UserName = jobSetting.scmSettings.FirstOrDefault().UserName,
                Password = jobSetting.scmSettings.FirstOrDefault().Password,
                Workspace = jobSetting.scmSettings.FirstOrDefault().Workspace,
                ViewMap = jobSetting.scmSettings.FirstOrDefault().ViewMap,
            };
        }

        internal static JobSetting ToJobSetting(this JobSettingDto jobSettingDto)
        {
            SCMSetting scmSetting = new SCMSetting()
            {
                SCMPort = jobSettingDto.SCMPort,
                UserName = jobSettingDto.UserName,
                Password = jobSettingDto.Password,
                Workspace = jobSettingDto.Workspace,
                ViewMap = jobSettingDto.ViewMap
            };
            var scmList = new List<SCMSetting>(); scmList.Add(scmSetting);
            return  new JobSetting()
            {
                JobName = jobSettingDto.JobName,
                buildPeriody = jobSettingDto.BuildPeriody,
                scmSettings = scmList
            };

            
        }
    }
}