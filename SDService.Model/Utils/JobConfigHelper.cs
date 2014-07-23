using SDService.Model.Basic;
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

        public static JobSetting parseJobSettingsfromXml(string xml, string projectName)
        {
            XDocument xDoc = XDocument.Parse(xml);
            JobSetting jSetting = new JobSetting();
            jSetting.ProjectName = projectName;
            jSetting.scmSettings = xDoc.Descendants("hudson.plugins.perforce.PerforceSCM").Select(
            SCMElement => new SCMSetting()
            {
                SCMPort = SCMElement.Element("p4Port").Value,
                UserName = SCMElement.Element("p4User").Value,
                Passoword = SCMElement.Element("p4Passwd").Value,
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

            Dictionary<string, string> colorConvertMap = new Dictionary<string, string>(){
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

            
            IEnumerable<Job> jobs = xDoc.Descendants("job").Select(
                JobElement => new Job()
                {
                    JobName = JobElement.Element("name").Value,
                    JobSettings = null,
                    Configuration = null,
                    LastBuild = null,
                    color = colorConvertMap[JobElement.Element("color").Value]
                }
                ).ToList<Job>();

            return jobs;
        }
    }
}
