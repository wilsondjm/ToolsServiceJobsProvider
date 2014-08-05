using Newtonsoft.Json;
using SDService.Model;
using SDService.Model.Basic;
using SDService.Model.Utils;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StringDetectorService.ReqResModel
{
    public class JobSettingDto
    {
        public string JobName { set; get; }
        public string BuildPeriody { set; get; }

        //public Test Test { get; set; }
        [JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
        public SCMSettingDto ScmSetting{set;get;}
    }

    public class Test
    {
        string name { get; set; }
    }

    public class SCMSettingDto{
    }

    public class PerforceSettingDto:SCMSettingDto
    {
        public string SCMPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Workspace { get; set; }
        public string ViewMap { get; set; }
    }

    public class GitSettingDto :SCMSettingDto
    {
        public string RepositoryUrl { get; set; }
        public string Name { get; set; }
        public string BranchSpecifier { get; set; }
    }

    public class SVNSettingDto : SCMSettingDto
    {
        public string RepositoryUrl { get; set; }
        public string LocalModulDir { get; set; }
    }

    internal static class JobSettingExtension
    {
        internal static Dictionary<Type, Constants.SCMType> ScmDtoTypeDict = new Dictionary<Type, Constants.SCMType>(){
            {typeof(PerforceSettingDto), Constants.SCMType.PERFORECE},
            {typeof(GitSettingDto), Constants.SCMType.GIT},
            {typeof(SVNSettingDto), Constants.SCMType.SVN}
        };
        
        internal static JobSettingDto ToJobSettingDto(this JobSetting jobSetting)
        {
            if (jobSetting == null)
            {
                return new JobSettingDto();
            }
            if (jobSetting.ScmSetting == null )
            {
                return new JobSettingDto()
                {

                    JobName = jobSetting.JobName,
                    BuildPeriody = jobSetting.BuildPeriody,
                };
            }
            return new JobSettingDto
            {
                JobName = jobSetting.JobName,
                BuildPeriody = jobSetting.BuildPeriody,
                ScmSetting = jobSetting.ScmSetting.ToSCMSettingDto()
            };
        }


        internal static SCMSettingDto ToSCMSettingDto(this SCMSetting setting)
        {

            switch (Constants.ScmTypeDict[setting.GetType()])
            {
                case Constants.SCMType.PERFORECE:
                    return new PerforceSettingDto
                    {
                        UserName = (setting as PerforceSetting).UserName,
                        Password = (setting as PerforceSetting).Password,
                        SCMPort = (setting as PerforceSetting).SCMPort,
                        Workspace = (setting as PerforceSetting).Workspace,
                        ViewMap = (setting as PerforceSetting).ViewMap,
                    };
                case Constants.SCMType.GIT:
                    return new GitSettingDto()
                    {
                        RepositoryUrl = (setting as GitSetting).RepositoryUrl,
                        Name = (setting as GitSetting).Name,
                        BranchSpecifier = (setting as GitSetting).BranchSpecifier,
                    };
                case Constants.SCMType.SVN:
                    return new SVNSettingDto()
                    {
                        RepositoryUrl = (setting as SVNSetting).RepositoryUrl,
                        LocalModulDir = (setting as SVNSetting).LocalModulDir,
                    };
                default:
                    return new SCMSettingDto()
                    {

                    };

            }
        }


        internal static JobSetting ToJobSetting(this JobSettingDto jobSettingDto, bool encryption = false)
        {
            
            JobSetting  setting =new JobSetting()
            {
                JobName = jobSettingDto.JobName,
                BuildPeriody = jobSettingDto.BuildPeriody,
            };

            if (jobSettingDto.ScmSetting != null)
            {
                switch (ScmDtoTypeDict[jobSettingDto.ScmSetting.GetType()])
                {
                    case Constants.SCMType.PERFORECE:
                        PerforceSettingDto perforceSetting = jobSettingDto.ScmSetting as PerforceSettingDto;

                        if (encryption)
                        {
                            setting.ScmSetting = new PerforceSetting()
                            {
                                UserName = perforceSetting.UserName,
                                Password = new PasswordEncryptionService().encryptString(perforceSetting.Password, HttpContext.Current.Server.MapPath(""), "\\..\\.."),
                                SCMPort = perforceSetting.SCMPort,
                                Workspace = perforceSetting.Workspace,
                                ViewMap = perforceSetting.ViewMap,
                            };
                        }

                        setting.ScmSetting = new PerforceSetting()
                        {
                            SCMPort = perforceSetting.SCMPort,
                            UserName = perforceSetting.UserName,
                            Password = perforceSetting.Password,
                            Workspace = perforceSetting.Workspace,
                            ViewMap = perforceSetting.ViewMap
                        };

                        break;
                    case Constants.SCMType.GIT:
                        GitSettingDto gitSetting = jobSettingDto.ScmSetting as GitSettingDto;
                        setting.ScmSetting = new GitSetting
                        {
                            RepositoryUrl = gitSetting.RepositoryUrl,
                            Name = gitSetting.Name,
                            BranchSpecifier = gitSetting.BranchSpecifier
                        };
                        break;
                    case Constants.SCMType.SVN:
                        SVNSettingDto svnSetting = jobSettingDto.ScmSetting as SVNSettingDto;
                        setting.ScmSetting = new SVNSetting
                        {
                            RepositoryUrl = svnSetting.RepositoryUrl,
                            LocalModulDir = svnSetting.LocalModulDir
                        };
                        break;
                }
            }
            return setting;
        }
    }
}