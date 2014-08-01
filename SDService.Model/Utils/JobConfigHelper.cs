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

        public static JobSetting parseJobSettingsfromXml(string xml, string jobName)
        {
            XDocument xDoc = XDocument.Parse(xml);
            JobSetting jSetting = new JobSetting();
            jSetting.JobName = jobName;
            jSetting.scmSettings = xDoc.Descendants("hudson.plugins.perforce.PerforceSCM").Select(
            SCMElement => new SCMSetting()
            {
                SCMPort = SCMElement.Element("p4Port").Value,
                UserName = SCMElement.Element("p4User").Value,
                Password = SCMElement.Element("p4Passwd").Value,
                Workspace = SCMElement.Element("p4Client").Value,
                ViewMap = SCMElement.Element("projectPath").Value,
            }).ToList<SCMSetting>();
            IEnumerable<string> buildPeriodyNodes = xDoc.Descendants("hudson.triggers.TimerTrigger").Select(
                    Element => Element.Element("spec").Value
                ).ToList<string>();
            jSetting.buildPeriody = buildPeriodyNodes.FirstOrDefault<string>();
            return jSetting;
        }

        public static IEnumerable<Job> parseJobsfromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);

            IEnumerable<Job> jobs = xDoc.Descendants("job").Select(
                JobElement => new Job()
                {
                    JobName = JobElement.Element("name").Value,
                    //JobSettings = new JobSetting() { JobName = JobElement.Element("name").Value },
                    //Configuration = new JobConfiguration() { JobName = JobElement.Element("name").Value },
                    //Builds = new JobHistory() { JobName = JobElement.Element("name").Value },
                    //LastBuild = new JobReport() { JobName = JobElement.Element("name").Value },
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


        public static IEnumerable<Tool> parseToolsfromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);

            IEnumerable<Tool> tools = xDoc.Descendants("view").Where(viewElement=>viewElement.Element("name").Value.StartsWith(Tool.Prifix)) .Select(
               
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


        public static IEnumerable<Project> parseProjectsfromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);

            IEnumerable<Project> projects = xDoc.Descendants("view").Where(viewElement => viewElement.Element("name").Value.StartsWith(Project.Prifix)).Select(

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
