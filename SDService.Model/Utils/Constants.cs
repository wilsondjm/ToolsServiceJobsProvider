using ServiceProvider.Model.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Model.Utils
{
    public static class Constants
    {
        public  const string defaultServerAddress = "10.158.2.6";
        public  const string defaultJenkinsServerAddress = "10.158.216.54:8080";
        public const string defaultAssignNode = "fakeone";

        public static readonly string projectSetting = "" //[ASSIGNEDNODE]  [UPSTREAMPROJECT]  [COMMAND]
            + "<project>"
            + "<actions/><description/><keepDependencies>false</keepDependencies><properties/>"
            + "<scm class=\"hudson.scm.NullSCM\"/>"
            + "<assignedNode>[ASSIGNEDNODE]</assignedNode>"
            + "<canRoam>false</canRoam><disabled>false</disabled><blockBuildWhenDownstreamBuilding>false</blockBuildWhenDownstreamBuilding><blockBuildWhenUpstreamBuilding>true</blockBuildWhenUpstreamBuilding>"
            + "<triggers><jenkins.triggers.ReverseBuildTrigger><spec/><upstreamProjects>[UPSTREAMPROJECT],</upstreamProjects><threshold><name>SUCCESS</name><ordinal>0</ordinal><color>BLUE</color><completeBuild>true</completeBuild></threshold></jenkins.triggers.ReverseBuildTrigger></triggers>"
            + "<concurrentBuild>false</concurrentBuild>"
            + "<builders><hudson.tasks.BatchFile>"
            + "<command>[COMMAND]</command>"
            + "</hudson.tasks.BatchFile></builders><publishers/><buildWrappers/>"
            + "</project>";

        // key-tool name value-command
        public static readonly Dictionary<string, string> ToolCommandMap = new Dictionary<string, string>()
        {
            {"faketool", "echo upstream project :[UPSTREAMPROJECT]  &amp; ping -n 40 127.0.0.1"}
        };

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
        public  const string StartActionFailed = "Start action failed";
        public  const string StopActionFailed = "Stop action failed";
        public  const string DeleteActionFailed = "The Delete action failed";
        public  const string CreateActionFailed = "Failed to create the job.";
        public  const string UpdateSettingsFailed = "Failed to update the job settings";
        public  const string AddConfigurationFailed = "Failed to add job configuration";
        public  const string UpdateConfigurationFailed = "Failed to update job configuration";
        public  const string DeleteConfigurationFailed = "Failed to delete job configuraion";
        public  const string DefaultConfiguration = " ";


        // job field constants
        public static readonly Collection<string> DefaultJobFields = new Collection<string> { JobNameField, JobSettingField, JobConfigField, JobHistoryField, JobReportField, JobStatusField };
        public  const string JobNameField = "jobname";
        public  const string JobSettingField = "setting";
        public  const string JobConfigField = "configuration";
        public  const string JobHistoryField = "builds";
        public  const string JobReportField = "report";
        public  const string JobStatusField = "status";

        //view field constants
        public static readonly Collection<string> DefaultToolFields = new Collection<string> { ViewNameField, NameField, JobsField };
        public  const string ViewNameField = "viewname";
        public  const string NameField = "name";
        public  const string JobsField = "jobs";


        // view category prefix
        public const string DefaultTag = "";
        public const string ToolTag = "Tool-";
        public const string ProjectTag = "Project-";


        // jenkins authentication
        public const string JenkinsUserName = "citrix";
        public const string JenkinsPassword = "citrix";

        //upstream project in jenkins job config xml
        public const string UpstreamProjectFlag = "upstreamProjects";
        // scm class string
        public const string GitSCM = "hudson.plugins.git.GitSCM";
        public const string PerforceSCM = "hudson.plugins.perforce.PerforceSCM";
        public const string SubversionSCM = "hudson.scm.SubversionSCM";

        public const string Git = "git";
        public const string SVN = "svn";
        public const string Perforce = "perforce";

        public  enum SCMType {GIT,SVN,PERFORECE }
        public static Dictionary<Type, SCMType> ScmTypeDict = new Dictionary<Type, SCMType>(){
            {typeof(PerforceSetting),SCMType.PERFORECE},
            {typeof(GitSetting),SCMType.GIT},
            {typeof(SVNSetting),SCMType.SVN}
        };
    }
}
