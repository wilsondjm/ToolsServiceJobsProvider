﻿using ServiceProvider.Client;
using ServiceProvider.Model;
using ServiceProvider.Model.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider.Service
{
    public class JobsService
    {
        JobsClient jobsClient;
        JobSettingClient settingClient;
        ConfigurationClient configClient;
        JobHistoryClient historyClient;
        JobReportsClient reportClient;
        JobStatusClient stausClient;

        public JobsService()
        {
            jobsClient = new JobsClient();
            settingClient = new JobSettingClient();
            configClient = new ConfigurationClient();
            historyClient = new JobHistoryClient();
            reportClient = new JobReportsClient();
            stausClient = new JobStatusClient();
        }

        public IEnumerable<Job> GetAllJobs(Collection<string> fields)
        {
            //More work needs to be done here to provide more job information
            // Get job name and stauts
            IList<Job> jobs = jobsClient.QueryAllSDJobs();
            JobProperties jobFields = new JobProperties(RequestFieldHelper.GetFirstLevelFields(fields));
            foreach (Job job in jobs)
            {
                
                if (jobFields.containsJobSetting)
                {
                    //Get JobSetting
                    job.Setting = settingClient.QueryJobSetting(job.JobName);
                }
                if (jobFields.containsJobConfig)
                {
                    //Get JobConfiguration
                    job.Configuration = configClient.getConfiguration(job.JobName);
                }
                if (jobFields.containsJobBuilds)
                {
                    //Get Job Build History
                    job.Builds = historyClient.getJobHistory(job.JobName);
                }
                if (jobFields.containsJobReport)
                {
                    //Get Job LastBuild
                    job.Report = reportClient.FetchReport(job.JobName);
                }
            }
            return jobs;
        }


        public Job GetJob(string jobName, Collection<string> fields)
        {
            Job job = new Job() { JobName=jobName};
            JobProperties jobFields = new JobProperties(RequestFieldHelper.GetFirstLevelFields(fields));
            if ( jobFields.containsJobStatus && jobFields.containsJobBuilds )
            {
                // get jobName jobStatus Builds
                job = jobsClient.QueryJob(jobName);
            }
            else
            {
                if (jobFields.containsJobStatus)
                {
                    job.Status = stausClient.getJobStatus(jobName);
                }
                if (jobFields.containsJobBuilds)
                {
                    job.Builds = historyClient.getJobHistory(jobName);
                }
            }
            
            if (jobFields.containsJobSetting)
            {
                //Get JobSetting
                job.Setting = settingClient.QueryJobSetting(job.JobName);
            }
            if (jobFields.containsJobConfig)
            {
                //Get JobConfiguration
                job.Configuration = configClient.getConfiguration(job.JobName);
            }
            if (jobFields.containsJobReport)
            {
                //Get Job LastBuild
                job.Report = reportClient.FetchReport(job.JobName);
            }
            return job;
        }



        public bool CreateJob(string jobName, string upstreamProject, string configuration)
        {
            //maybe should create configuration first
           // return (configClient.addConfiguration(jobName, configuration) && jobsClient.createJob(jobName, upstreamProject));
            // temporary disable create job configuration when creating job
            return ( jobsClient.createJob(jobName, upstreamProject));
        }

        public bool deleteJob (string projectName)
        {
            return jobsClient.DeleteJob(projectName);
        }
    }
   
}
