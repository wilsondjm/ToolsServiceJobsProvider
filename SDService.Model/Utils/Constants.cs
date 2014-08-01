using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDService.Model.Utils
{
    public static class Constants
    {
        public static const string defaultServerAddress = "10.158.216.54";
        public static const string defaultJenkinsServerAddress = "10.158.216.54:8080";
        public static readonly string perforceSetting = ""  // [P4USERNAME], [P4PASSWORD], [P4PORT], [P4WORKSPACE], [PROJECTPATH]
          + "<hudson.plugins.perforce.PerforceSCM plugin=\"perforce@1.3.27\">"
          + "<configVersion>1</configVersion><p4User>[P4USERNAME]</p4User><p4Passwd>[P4PASSWORD]</p4Passwd><p4Port>[P4PORT]</p4Port><p4Client>[P4WORKSPACE]</p4Client>"
          + "<projectPath>"
          + "[PROJECTPATH]"
            //+         "//prodlic/develop/main/src/LSMetaInstallerUI/... //vincenthu_NKGWVINCENTHU_Lic_SD/LSMetaInstallerUI/... "
            //+         "//prodlic/develop/main/src/Jazz/UI/... //vincenthu_NKGWVINCENTHU_Lic_SD/Jazz/UI/..."
            //+         "//prodlic/develop/main/src/Tango/UI/... //vincenthu_NKGWVINCENTHU_Lic_SD/Tango/UI/..."
          + "</projectPath>"
          + "<projectOptions>noallwrite clobber nocompress unlocked nomodtime rmdir</projectOptions>"
          + "<clientOwner/><p4SysDrive/><p4SysRoot/><p4Tool/><useClientSpec>false</useClientSpec><useStreamDepot>false</useStreamDepot><forceSync>false</forceSync><alwaysForceSync>false</alwaysForceSync><dontUpdateServer>false</dontUpdateServer><disableAutoSync>false</disableAutoSync><disableChangeLogOnly>false</disableChangeLogOnly><disableSyncOnly>false</disableSyncOnly><showIntegChanges>false</showIntegChanges><useOldClientName>false</useOldClientName><createWorkspace>true</createWorkspace><updateView>true</updateView><dontRenameClient>false</dontRenameClient><updateCounterValue>false</updateCounterValue><dontUpdateClient>false</dontUpdateClient><exposeP4Passwd>false</exposeP4Passwd><wipeBeforeBuild>false</wipeBeforeBuild><quickCleanBeforeBuild>false</quickCleanBeforeBuild><restoreChangedDeletedFiles>false</restoreChangedDeletedFiles><wipeRepoBeforeBuild>false</wipeRepoBeforeBuild><firstChange>-1</firstChange><fileLimit>0</fileLimit><excludedFilesCaseSensitivity>true</excludedFilesCaseSensitivity><slaveClientNameFormat>${basename}-${hash}</slaveClientNameFormat><lineEndValue>local</lineEndValue><useViewMask>true</useViewMask><useViewMaskForPolling>true</useViewMaskForPolling><useViewMaskForSyncing>false</useViewMaskForSyncing><useViewMaskForChangeLog>false</useViewMaskForChangeLog><pollOnlyOnMaster>false</pollOnlyOnMaster>"
          + "</hudson.plugins.perforce.PerforceSCM>";

        public static readonly string projectSetting = "" //[SCMSETTING]  [BUILDPERIODY]  [PROJECTNAME]
            + "<project>"
            + "<actions/><description/><keepDependencies>false</keepDependencies><properties/>"
            + "<scm class=\"org.jenkinsci.plugins.multiplescms.MultiSCM\" plugin=\"multiple-scms@0.3\">"
            + "<scms>"
            + "[SCMSETTING]"
            + "<hudson.plugins.perforce.PerforceSCM plugin=\"perforce@1.3.27\"><configVersion>1</configVersion><p4User>vincenthu</p4User><p4Passwd>0f0kqlwacTcERBHEHkvfa3AlrK8xqw==</p4Passwd><p4Port>NKGP401.eng.citrite.net:2444</p4Port><p4Client>vincenthu_NKGWVINCENTHU_TEST_SD</p4Client><projectPath>//localization/LocEngineering/LocEngg-Tools/String_Detector/dist/... //vincenthu_NKGWVINCENTHU_TEST_SD/...</projectPath><projectOptions>noallwrite clobber nocompress unlocked nomodtime rmdir</projectOptions><p4SysDrive/><p4SysRoot/><p4Tool/><useClientSpec>false</useClientSpec><useStreamDepot>false</useStreamDepot><forceSync>false</forceSync><alwaysForceSync>false</alwaysForceSync><dontUpdateServer>false</dontUpdateServer><disableAutoSync>false</disableAutoSync><disableChangeLogOnly>false</disableChangeLogOnly><disableSyncOnly>false</disableSyncOnly><showIntegChanges>false</showIntegChanges><useOldClientName>false</useOldClientName><createWorkspace>true</createWorkspace><updateView>true</updateView><dontRenameClient>false</dontRenameClient><updateCounterValue>false</updateCounterValue><dontUpdateClient>false</dontUpdateClient><exposeP4Passwd>false</exposeP4Passwd><wipeBeforeBuild>false</wipeBeforeBuild><quickCleanBeforeBuild>false</quickCleanBeforeBuild><restoreChangedDeletedFiles>false</restoreChangedDeletedFiles><wipeRepoBeforeBuild>false</wipeRepoBeforeBuild><firstChange>0</firstChange><fileLimit>0</fileLimit><excludedFilesCaseSensitivity>true</excludedFilesCaseSensitivity><slaveClientNameFormat>${basename}-${hash}</slaveClientNameFormat><lineEndValue>local</lineEndValue><useViewMask>true</useViewMask><useViewMaskForPolling>true</useViewMaskForPolling><useViewMaskForSyncing>false</useViewMaskForSyncing><useViewMaskForChangeLog>false</useViewMaskForChangeLog><pollOnlyOnMaster>false</pollOnlyOnMaster></hudson.plugins.perforce.PerforceSCM>"
            + "</scms>"
            + "</scm><canRoam>true</canRoam><disabled>false</disabled><blockBuildWhenDownstreamBuilding>false</blockBuildWhenDownstreamBuilding><blockBuildWhenUpstreamBuilding>false</blockBuildWhenUpstreamBuilding>"
            + "<triggers><hudson.triggers.TimerTrigger><spec>" + "[BUILDPERIODY]" + "</spec></hudson.triggers.TimerTrigger></triggers>"
            + "<concurrentBuild>false</concurrentBuild>"
            + "<builders><hudson.tasks.BatchFile>"
            + "<command>xcopy C:\\Projects\\StringDetector\\Configurations\\" + "[PROJECTNAME] \"%WORKSPACE%\\\" /c /y \n\"%WORKSPACE%/string_detector.exe\" .</command>"
            + "</hudson.tasks.BatchFile></builders><publishers/><buildWrappers/>"
            + "</project>";

        public static readonly Dictionary<string, string> ColorConvertMap = new Dictionary<string, string>(){
             {"red","Failed"},
             {"red_anime","InProgress"},
             {"yellow","Unstable"},
             {"yellow_anime","InProgress"},
             {"blue","Success"},
             {"blue_anime","InProgress"},
             {"grey","Pending"},
             {"grey_anime","InProgress"},
             {"disabled","Disabled"},
             {"disabled_anime","InProgress"},
             {"aborted","Aborted"},
             {"aborted_anime","InProgress"},
             {"notbuilt","NotBuilt"},
             {"notbuilt_anime","InProgress"},
            };

        // job action constants
        public static const string StartActionFailed = "Start action failed";
        public static const string StopActionFailed = "Stop action failed";
        public static const string DeleteActionFailed = "The Delete action failed";
        public static const string CreateActionFailed = "Failed to create the job.";
        public static const string UpdateSettingsFailed = "Failed to update the job settings";
        public static const string AddConfigurationFailed = "Failed to add job configuration";
        public static const string UpdateConfigurationFailed = "Failed to update job configuration";
        public static const string DeleteConfigurationFailed = "Failed to delete job configuraion";
        public static const string DefaultConfiguration = " ";


        // job field constants
        public static readonly Collection<string> DefaultJobFields = new Collection<string> { JobNameField, JobSettingField, JobConfigField, JobHistoryField, JobReportField, JobStatusField };
        public static const string JobNameField = "jobname";
        public static const string JobSettingField = "setting";
        public static const string JobConfigField = "configuration";
        public static const string JobHistoryField = "builds";
        public static const string JobReportField = "report";
        public static const string JobStatusField = "status";

        //tool field constants
        public static readonly Collection<string> DefaultToolFields = new Collection<string> { ViewNameField, ToolNameField, JobsField };
        public static const string ViewNameField = "viewname";
        public static const string ToolNameField = "toolname";
        public static const string JobsField = "jobs";


    }
}
