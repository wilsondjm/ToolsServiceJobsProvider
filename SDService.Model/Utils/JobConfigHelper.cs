using SDService.Model.Basic;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SDService.Model.Utils
{
    public class JobConfigHelper
    {
        public static string getP4SingleDepotJobConfig(string projectName, string p4User, string p4Passwd, string p4Port, string p4Workspace, string projectPath, string buildPeriody)
        {
            StringBuilder scmString = new StringBuilder(Constants.perforceSetting);
            scmString.Replace("[P4USERNAME]", p4User);
            scmString.Replace("[P4PASSWORD]", p4Passwd);
            scmString.Replace("[P4PORT]", p4Port);
            scmString.Replace("[P4WORKSPACE]", p4Workspace);
            scmString.Replace("[PROJECTPATH]", projectPath);

            StringBuilder p4JobConfig = new StringBuilder(Constants.projectSetting);
            p4JobConfig.Replace("[BUILDPERIODY]", buildPeriody);
            p4JobConfig.Replace("[PROJECTNAME]", projectName);
            p4JobConfig.Replace("[SCMSETTING]", scmString.ToString());

            return p4JobConfig.ToString();
        }




        public static string updatePerforceSetting( XDocument xDoc,PerforceSetting setting,PerforceSettingProperties properties)
        {
            XElement scmElement= xDoc.Descendants("scm").FirstOrDefault();
            // update scm port
            if (properties.containsSCMPort)
            {
                scmElement.Element("p4Port").Value = setting.SCMPort;
            }
            // update username
            if (properties.containsUserName)
            {
                scmElement.Element("p4User").Value = setting.UserName;
            }
            // update password
            if (properties.containsPassword)
            {
                scmElement.Element("p4Passwd").Value = setting.Password;
            }
            // update workspace
            if (properties.containsWorkspace)
            {
                scmElement.Element("p4Client").Value = setting.Workspace;
            }
           // update viewmap
            if (properties.containsViewMap)
            {
                scmElement.Element("projectPath").Value = setting.ViewMap;
            }
            return xDoc.ToString();
        }

        public static string updateSVNSetting( XDocument xDoc,SVNSetting setting, SVNSettingProperties properties)
        {
            XElement scmElement = xDoc.Descendants("scm").FirstOrDefault();
            XElement locationElement = xDoc.Descendants("hudson.scm.SubversionSCM_-ModuleLocation").FirstOrDefault();
            // update repository url
            if (properties.containsRepositoryUrl)
            {
                locationElement.Element("remote").Value = setting.RepositoryUrl;
            }
            // update local modul dir
            if (properties.containsLocalModulDir)
            {
                XElement localElement = locationElement.Element("local");
                if (setting.LocalModulDir == null || setting.LocalModulDir == "")
                {
                    // if alreay exsist ,delete ;else do nothing
                    if (localElement != null)
                    {
                        localElement.Remove();
                    }
                }
                else
                {
                    // if alreay exsist ,change ;else  add such element
                    if (localElement != null)
                    {
                        localElement.Value = setting.LocalModulDir;
                    }
                    else
                    {
                        locationElement.Add(new XElement("local", setting.LocalModulDir));
                    }
                }
            }
            // update repository depth
            if (properties.containsRepositoryDepth)
            {
                locationElement.Element("depthOption").Value = setting.RepositoryDepth;
            }
           // update  ignore external
            if (properties.containsIgnoreExternals)
            {
                locationElement.Element("ignoreExternalsOption").Value = setting.IgnoreExternals;
            }
            // update exclude regions
            if (properties.containsExcludedRegions)
            {
                scmElement.Element("excludedRegions").Value = setting.ExcludedRegions;
            }
            // update icclude regions
            if (properties.containsIncludeRegions)
            {
                scmElement.Element("includedRegions").Value = setting.IncludeRegions;
            }
            // update exclude users
            if (properties.containsExcludedUsers)
            {
                scmElement.Element("excludedUsers").Value = setting.ExcludedUsers;
            }
            // update exclusion revprop name
            if (properties.containsExclusionRevpropName)
            {
                scmElement.Element("excludedRevprop").Value = setting.ExclusionRevpropName;
            }
            // update exclude commit message
            if (properties.containsExcludedCommitMessages)
            {
                scmElement.Element("excludedCommitMessages").Value = setting.ExcludedCommitMessages;
            }
            return xDoc.ToString();
        }

        public static string updateGitSetting(XDocument xDoc, GitSetting setting, GitSettingProperties properties)
        {

            XElement remoteElement = xDoc.Descendants("hudson.plugins.git.UserRemoteConfig").FirstOrDefault();
            XElement extensionElement = xDoc.Descendants("hudson.plugins.git.extensions.impl.SubmoduleOption").FirstOrDefault();
            XElement branchElement = xDoc.Descendants("hudson.plugins.git.BranchSpec").FirstOrDefault();
            // update repository url
            if (properties.containsRepositoryUrl)
            {
                remoteElement.Element("url").Value = setting.RepositoryUrl;
            }
            // update name 
            if (properties.containsName)
            {
                XElement nameElement = remoteElement.Element("name");
                if (setting.Name == null || setting.Name == "")
                {
                    if (nameElement != null)
                    {
                        nameElement.Remove();
                    }
                }
                else
                {
                    if (nameElement != null)
                    {
                        nameElement.Value = setting.Name;
                    }
                    else
                    {
                        remoteElement.Add(new XElement("name", setting.Name));
                    }
                }
            }
            // update branch specifier
            if (properties.containsBranchSpecifier)
            {
                branchElement.Element("name").Value = setting.BranchSpecifier;
                XElement credentialElement = remoteElement.Element("credentialsId");
                if (setting.CredentialsId == null || setting.CredentialsId == "")
                {
                    if (credentialElement != null)
                    {
                        credentialElement.Remove();
                    }
                }
                else
                {
                    if (credentialElement != null)
                    {
                        credentialElement.Value = setting.CredentialsId;
                    }
                    else
                    {
                        remoteElement.Add(new XElement("credentialsId", setting.CredentialsId));
                    }
                }
            }
            // update refspec
            if (properties.containsRefspec)
            {
                XElement refSpecElement = remoteElement.Element("refspec");
                if (setting.Refspec == null || setting.Refspec == "")
                {
                    if (refSpecElement != null)
                    {
                        refSpecElement.Remove();
                    }
                }
                else
                {
                    if (refSpecElement != null)
                    {
                        refSpecElement.Value = setting.Refspec;
                    }
                    else
                    {
                        remoteElement.Add(new XElement("refspec", setting.Refspec));
                    }
                }
            }
            // update disable submodoules
            if (properties.containsDisableSubmodules)
            {
                extensionElement.Element("disableSubmodules").Value = setting.DisableSubmodules;
            }
            // update recursive submodules
            if (properties.containsRecursiveSubmodules)
            {
                extensionElement.Element("recursiveSubmodules").Value = setting.RecursiveSubmodules;
            }
            // update trancing submodules
            if (properties.containsTrackingSubmodules)
            {
                extensionElement.Element("trackingSubmodules").Value = setting.TrackingSubmodules;
            }
            return xDoc.ToString();

        }


        public static string updateJobSetting(string xml, JobSetting jSetting,JobSettingProperties properties)
        {
            XDocument xDoc = XDocument.Parse(xml);
            // update build period
            if (properties.containsBuildPeriody)
            {
                XElement triggersElement = xDoc.Descendants("triggers").FirstOrDefault();
                XElement buildPeriodyNode = triggersElement.Element("hudson.triggers.TimerTrigger");
                if (buildPeriodyNode == null)
                {
                    triggersElement.Add(new XElement("hudson.triggers.TimerTrigger"));
                    buildPeriodyNode = triggersElement.Element("hudson.triggers.TimerTrigger"); 
                }
                XElement specElement = buildPeriodyNode.Element("spec");
                if (specElement == null)
                {
                    buildPeriodyNode.Add(new XElement("spec", jSetting.BuildPeriody));
                }
                else
                {
                    specElement.Value = jSetting.BuildPeriody;
                }
            }
            // update scmsetting 
            if (properties.containsScmSetting)
            {
                switch (Constants.ScmTypeDict[jSetting.ScmSetting.GetType()])
                {
                    case Constants.SCMType.SVN:
                        return JobConfigHelper.updateSVNSetting(xDoc, jSetting.ScmSetting as SVNSetting ,properties.scmSettingProperties as SVNSettingProperties);
                    case Constants.SCMType.GIT:
                        return JobConfigHelper.updateGitSetting(xDoc, jSetting.ScmSetting as GitSetting,properties.scmSettingProperties as GitSettingProperties);
                    case Constants.SCMType.PERFORECE:
                        return JobConfigHelper.updatePerforceSetting(xDoc, jSetting.ScmSetting as PerforceSetting, properties.scmSettingProperties as PerforceSettingProperties);
                    default:
                        return xDoc.ToString();
                }
            }
            return xDoc.ToString();
            
        }

        public static JobSetting parseCommonJobSettingsfromXml(string xml, string jobName)
        {
            XDocument xDoc = XDocument.Parse(xml);
            JobSetting jSetting = new JobSetting();
            jSetting.JobName = jobName;

            string scm = xDoc.Root.Element("scm").Attribute("class").Value;
            switch (scm)
            {
                case Constants.PerforceSCM:
                    jSetting.ScmSetting = xDoc.Descendants("scm").Select(
                       SCMElement => new PerforceSetting()
                       {
                           SCMPort = SCMElement.Element("p4Port").Value,
                           UserName = SCMElement.Element("p4User").Value,
                           Password = SCMElement.Element("p4Passwd").Value,
                           Workspace = SCMElement.Element("p4Client").Value,
                           ViewMap = SCMElement.Element("projectPath").Value,
                       }).FirstOrDefault();
                    break;
                case Constants.SubversionSCM:
                    jSetting.ScmSetting = xDoc.Descendants("scm").Select(
                      SCMElement => new SVNSetting()
                      {
                          RepositoryUrl = xDoc.Descendants("hudson.scm.SubversionSCM_-ModuleLocation").Select(locationElement => locationElement.Element("remote").Value).FirstOrDefault(),
                          LocalModulDir =xDoc.Descendants("hudson.scm.SubversionSCM_-ModuleLocation").Select(locationElement => locationElement.Element("local")!=null?locationElement.Element("local").Value:"any").FirstOrDefault(),
                          //optional content
                          RepositoryDepth = xDoc.Descendants("hudson.scm.SubversionSCM_-ModuleLocation").Select(locationElement => locationElement.Element("depthOption").Value).FirstOrDefault(),
                          IgnoreExternals = xDoc.Descendants("hudson.scm.SubversionSCM_-ModuleLocation").Select(locationElement => locationElement.Element("ignoreExternalsOption").Value).FirstOrDefault(),
                          ExcludedRegions = SCMElement.Element("excludedRegions").Value,
                          IncludeRegions =SCMElement.Element("includedRegions").Value,
                          ExcludedUsers = SCMElement.Element("excludedUsers").Value,
                          ExclusionRevpropName = SCMElement.Element("excludedRevprop").Value,
                          ExcludedCommitMessages = SCMElement.Element("excludedCommitMessages").Value
                          
                      }).FirstOrDefault();
                    break;
                case Constants.GitSCM:

                    jSetting.ScmSetting = xDoc.Descendants("scm").Select(
                      SCMElement => new GitSetting()
                      {
                          RepositoryUrl = xDoc.Descendants("hudson.plugins.git.UserRemoteConfig").Select(remoteElement => remoteElement.Element("url").Value).FirstOrDefault(),
                          Name = xDoc.Descendants("hudson.plugins.git.UserRemoteConfig").Select(remoteElement => remoteElement.Element("name") != null ? remoteElement.Element("name").Value : "none").FirstOrDefault(),
                          BranchSpecifier = xDoc.Descendants("hudson.plugins.git.BranchSpec").Select(branchElement => branchElement.Element("name").Value).FirstOrDefault(),

                          //optional content
                          CredentialsId = xDoc.Descendants("hudson.plugins.git.UserRemoteConfig").Select(remoteElement => remoteElement.Element("credentialsId")!=null? remoteElement.Element("credentialsId").Value:"none").FirstOrDefault(),
                          Refspec = xDoc.Descendants("hudson.plugins.git.UserRemoteConfig").Select(remoteElement => remoteElement.Element("refspec")!= null ? remoteElement.Element("refspec").Value: "none").FirstOrDefault(),
                          DisableSubmodules = xDoc.Descendants("hudson.plugins.git.extensions.impl.SubmoduleOption").Select(extensionElement => extensionElement.Element("disableSubmodules").Value).FirstOrDefault(),
                          RecursiveSubmodules = xDoc.Descendants("hudson.plugins.git.extensions.impl.SubmoduleOption").Select(extensionElement => extensionElement.Element("recursiveSubmodules").Value).FirstOrDefault(),
                          TrackingSubmodules = xDoc.Descendants("hudson.plugins.git.extensions.impl.SubmoduleOption").Select(extensionElement => extensionElement.Element("trackingSubmodules").Value).FirstOrDefault(),

                      }).FirstOrDefault();
                    break;
            }


           
            IEnumerable<string> buildPeriodyNodes = xDoc.Descendants("hudson.triggers.TimerTrigger").Select(
                    Element => Element.Element("spec").Value
                ).ToList<string>();
            jSetting.BuildPeriody = buildPeriodyNodes.FirstOrDefault<string>();
            return jSetting;
        }


        private static IEnumerable<SCMSetting> getMultipleSCMSetting(XDocument xDoc)
        {
            return xDoc.Descendants("hudson.plugins.perforce.PerforceSCM").Select(
            SCMElement => new PerforceSetting()
            {
                SCMPort = SCMElement.Element("p4Port").Value,
                UserName = SCMElement.Element("p4User").Value,
                Password = SCMElement.Element("p4Passwd").Value,
                Workspace = SCMElement.Element("p4Client").Value,
                ViewMap = SCMElement.Element("projectPath").Value,
            });
        }


        public static string parseUpStreamProjectfromXml(string xml){
             XDocument xDoc = XDocument.Parse(xml);
             string upStreamProject = xDoc.Descendants("upstreamProjects").Select(
                     Element => Element.Value
                 ).First().Split(',').First();
             return upStreamProject;
        }


       // public static 

        public static IList<Job> parseJobsfromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);

            IList<Job> jobs = xDoc.Descendants("job").Select(
                JobElement => new Job()
                {
                    JobName = JobElement.Element("name").Value,
                    Status = new JobStatus(){JobName = JobElement.Element("name").Value,
                                             Status = Constants.ColorConvertMap[JobElement.Element("color").Value]
                    } 
                }
                ).ToList<Job>();

            return jobs;
        }





        public static Job parseJobFromXml(string xml,string jobName)
        {
            Job job = new Job() { JobName = jobName ,Builds = new JobHistory(){JobName = jobName},Status = new JobStatus(){JobName=jobName} };
            XDocument xDoc = XDocument.Parse(xml);
            var status = Constants.ColorConvertMap[xDoc.Root.Element("color").Value];
            job.Status.Status = status;
            if (status.Equals("NotBuilt", StringComparison.InvariantCultureIgnoreCase))
            {
                //no build history for this project
                return job;
            }

            job.Builds.JobHistories = xDoc.Descendants("build").Select(
                build => new HistoryItem()
                {
                    Duration = build.Element("duration").Value,
                    FullDisplayName = build.Element("fullDisplayName").Value,
                    Id = build.Element("id").Value,
                    Number = build.Element("number").Value,
                    Result = build.Element("result") == null ? "Pending" : build.Element("result").Value
                }
            ).ToList<HistoryItem>();
            XElement lastBuildElement = xDoc.Root.Element("lastBuild");
            job.Builds.LastBuild = new HistoryItem()
            {
                Duration = lastBuildElement.Element("duration").Value,
                FullDisplayName = lastBuildElement.Element("fullDisplayName").Value,
                Id = lastBuildElement.Element("id").Value,
                Number = lastBuildElement.Element("number").Value,
                Result = lastBuildElement.Element("result") == null ? "Pending" : lastBuildElement.Element("result").Value
            };

            return job;
        }


        public static IList<Tool> parseToolsfromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);

            IList<Tool> tools = xDoc.Descendants("view").Where(viewElement=>viewElement.Element("name").Value.StartsWith(Tool.Prifix)) .Select(
               
                toolElement => new Tool()
                {
                    ViewName = toolElement.Element("name").Value,
                    ToolName = toolElement.Element("name").Value.Replace(Tool.Prifix,"")
                    
                }
                ).ToList<Tool>();

            return tools;
        }

        public static Tool parseToolfromXml(string xml,string toolName)
        {
            XDocument xDoc = XDocument.Parse(xml);
            Tool tool = new Tool() { ViewName = Tool.Prifix + toolName, ToolName = toolName };
            tool.Jobs = xDoc.Descendants("job").Select(
               JobElement => new Job()
               {
                   JobName = JobElement.Element("name").Value,
                   Status = new JobStatus()
                   {
                       JobName = JobElement.Element("name").Value,
                       Status = Constants.ColorConvertMap[JobElement.Element("color").Value]
                   }
               }
               ).ToList<Job>();

            return tool;
        }


        public static IList<Project> parseProjectsfromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);

            IList<Project> projects = xDoc.Descendants("view").Where(viewElement => viewElement.Element("name").Value.StartsWith(Project.Prifix)).Select(

                projectElement => new Project()
                {
                    ViewName = projectElement.Element("name").Value,
                    ProjectName = projectElement.Element("name").Value.Replace(Project.Prifix, "")

                }
                ).ToList<Project>();

            return projects;
        }
    }
}
