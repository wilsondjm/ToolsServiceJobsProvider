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
        ConfigurationClient configClient;
        JobStatusReportsClient statusReportClient;

        public JobsService()
        {
            jobsClient = new JobsClient();
            configClient = new ConfigurationClient();
            statusReportClient = new JobStatusReportsClient();
        }

        public IEnumerable<Job> GetAllJobs()
        {
            //More work needs to be done here to provide more job information
            IEnumerable<Job> jobs = jobsClient.QueryAllSDJobs();
            foreach (Job job in jobs)
            {
                job.LastBuild = statusReportClient.FetchReport(job.JobName);
            }
            return jobs;
               
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

        public bool updateJobConfig(JobSetting jobSetting)
        {
            return jobsClient.UpdateJob(jobSetting);
        }

        public JobSetting readJobConfig(string jobName)
        {
            return jobsClient.QueryJobSetting(jobName);
        }
    }
}
