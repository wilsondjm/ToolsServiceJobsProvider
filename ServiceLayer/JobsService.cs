using RequestClient;
using SDService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class JobsService
    {
        JobsClient jobsClient;
        JobSettingClient settingClient;
        ConfigurationClient configClient;
        JobHistoryClient historyClient;
        JobReportsClient reportClient;

        public JobsService()
        {
            jobsClient = new JobsClient();
            settingClient = new JobSettingClient();
            configClient = new ConfigurationClient();
            historyClient = new JobHistoryClient();
            reportClient = new JobReportsClient();
        }

        public IEnumerable<Job> GetAllJobs()
        {
            //More work needs to be done here to provide more job information
            // Get job name and stauts
            IEnumerable<Job> jobs = jobsClient.QueryAllSDJobs();

            foreach (Job job in jobs)
            {
                //Get JobSetting
                job.JobSettings =settingClient.QueryJobSetting(job.JobName);
                //Get JobConfiguration
                job.Configuration = configClient.getConfiguration(job.JobName);
                //Get Job Build History
                job.Builds = historyClient.getJobHistory(job.JobName);
                //Get Job LastBuild
                job.LastBuild = reportClient.FetchReport(job.JobName);
            }
            return jobs;
        }


        public Job GetJob(string jobName)
        {
            // get jobName jobStatus Builds
            Job job  = jobsClient.QueryJob(jobName);
            //Get JobSetting
            job.JobSettings = settingClient.QueryJobSetting(job.JobName);
            //Get JobConfiguration
            job.Configuration = configClient.getConfiguration(job.JobName);
            //Get Job LastBuild
            job.LastBuild = reportClient.FetchReport(job.JobName);
            return job;
        }



        public bool CreateJob (JobSetting jobSetting, JobConfiguration configuration)
        {
            //maybe should create configuration first
            return (configClient.addConfiguration(configuration.JobName, configuration.Configuration) && jobsClient.createJob(jobSetting));
        }

        public bool deleteJob (string projectName)
        {
            return jobsClient.DeleteJob(projectName);
        }

       
    }
}
