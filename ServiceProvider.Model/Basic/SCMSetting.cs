using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Model.Basic
{
    public class SCMSetting
    {
        
    }

    public class PerforceSetting :SCMSetting
    {
        public string SCMPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Workspace { get; set; }
        public string ViewMap { get; set; }

        //optional content go there
    }

    public class GitSetting : SCMSetting
    {
        public string RepositoryUrl { get; set; }
        public string Name { get; set; }
        public string BranchSpecifier { get; set; }

        // optional content go there
        public string CredentialsId { get; set; }
        public string Refspec { get; set; }
        public string DisableSubmodules { get; set; }
        public string RecursiveSubmodules { get; set; }
        public string TrackingSubmodules { get; set; }
    }

    public class SVNSetting : SCMSetting
    {
        public string RepositoryUrl { get; set; }
        public string LocalModulDir { get; set; }

        //optional content go there
        public string RepositoryDepth { get; set; }
        public string IgnoreExternals { get; set; }
        public string ExcludedRegions { get; set; }
        public string IncludeRegions { get; set; }
        public string ExcludedUsers { get; set; }
        public string ExcludedCommitMessages { get; set; }
        public string ExclusionRevpropName { get; set; }
    }

    public abstract class SCMSettingProperties
    {
        public SCMSettingProperties(SCMSetting setting)
        {
            setScmProperties(setting);
        }
        public abstract void setScmProperties(SCMSetting setting);
    }

    public class PerforceSettingProperties : SCMSettingProperties
    {
        public bool containsSCMPort;
        public bool containsUserName;
        public bool containsPassword;
        public bool containsWorkspace;
        public bool containsViewMap;

        public PerforceSettingProperties(SCMSetting setting) : base(setting) { }

        public override void setScmProperties(SCMSetting setting)
        {
            PerforceSetting pSetting = setting as PerforceSetting;
            if (pSetting.UserName != null)
            {
                containsUserName = true;
            }
            if (pSetting.Password != null)
            {
                containsPassword = true;
            }
            if (pSetting.SCMPort != null)
            {
                containsSCMPort = true;
            }
            if (pSetting.Workspace != null)
            {
                containsWorkspace = true;
            }
            if (pSetting.ViewMap != null)
            {
                containsViewMap = true;
            }
        }
    }

    public class SVNSettingProperties : SCMSettingProperties
    {
        public bool containsRepositoryUrl;
        public bool containsLocalModulDir;

        //optional content go there
        public bool containsRepositoryDepth;
        public bool containsIgnoreExternals;
        public bool containsExcludedRegions;
        public bool containsIncludeRegions;
        public bool containsExcludedUsers;
        public bool containsExcludedCommitMessages;
        public bool containsExclusionRevpropName;
        public SVNSettingProperties(SCMSetting setting) : base(setting) { }
        public override void setScmProperties(SCMSetting setting)
        {
            SVNSetting sSetting = setting as SVNSetting;
            if (sSetting.RepositoryUrl != null)
            {
                containsRepositoryUrl = true;
            }
            if (sSetting.LocalModulDir != null)
            {
                containsLocalModulDir = true;
            }
            if (sSetting.RepositoryDepth != null)
            {
                containsRepositoryDepth = true;
            }
            if (sSetting.IgnoreExternals != null && (sSetting.IgnoreExternals == "true" || sSetting.IgnoreExternals == "false"))
            {
                containsIgnoreExternals = true;
            }
            if (sSetting.ExcludedRegions != null)
            {
                containsExcludedRegions = true;
            }
            if (sSetting.IncludeRegions != null)
            {
                containsIncludeRegions = true;
            }
            if (sSetting.ExcludedUsers != null)
            {
                containsExcludedUsers = true;
            }
            if (sSetting.ExcludedCommitMessages != null)
            {
                containsExcludedCommitMessages = true;
            }
            if (sSetting.ExclusionRevpropName != null)
            {
                containsExclusionRevpropName = true;
            }
        }
    }
    public class GitSettingProperties : SCMSettingProperties
    {
        public bool containsRepositoryUrl;
        public bool containsName;
        public bool containsBranchSpecifier;

        // optional content go there
        public bool containsCredentialsId;
        public bool containsRefspec;
        public bool containsDisableSubmodules;
        public bool containsRecursiveSubmodules;
        public bool containsTrackingSubmodules;
        public GitSettingProperties(SCMSetting setting) : base(setting) { }

        public override void setScmProperties(SCMSetting setting)
        {
            GitSetting gSetting = setting as GitSetting;
            if (gSetting.RepositoryUrl != null)
            {
                containsRepositoryUrl = true;
            }
            if (gSetting.Name != null)
            {
                containsName = true;
            }
            if (gSetting.BranchSpecifier != null)
            {
                containsBranchSpecifier = true;
            }
            if (gSetting.CredentialsId != null)
            {
                containsCredentialsId = true;
            }
            if (gSetting.Refspec != null)
            {
                containsRefspec = true;
            }
            if (gSetting.DisableSubmodules != null && (gSetting.DisableSubmodules == "true" || gSetting.DisableSubmodules == "false"))
            {
                containsDisableSubmodules = true;
            }
            if (gSetting.RecursiveSubmodules != null && (gSetting.RecursiveSubmodules == "true" || gSetting.RecursiveSubmodules == "false"))
            {
                containsRecursiveSubmodules = true;
            }
            if (gSetting.TrackingSubmodules != null && (gSetting.TrackingSubmodules == "true" || gSetting.TrackingSubmodules == "false"))
            {
                containsTrackingSubmodules = true;
            }
        }
    }
}
