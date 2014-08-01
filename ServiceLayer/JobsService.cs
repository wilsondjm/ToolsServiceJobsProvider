﻿using RequestClient;
using SDService.Model;
using SDService.Model.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            JobFieldProperties jobFields = new JobFieldProperties(RequestFieldHelper.GetFirstLevelFields(fields));
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
            JobFieldProperties jobFields = new JobFieldProperties(RequestFieldHelper.GetFirstLevelFields(fields));
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
    class JobFieldProperties
    {
        public bool containsJobName = false;
        public bool containsJobSetting = false;
        public bool containsJobConfig = false;
        public bool containsJobBuilds = false;
        public bool containsJobReport = false;
        public bool containsJobStatus = false;

        public JobFieldProperties(Collection<string> fields)
        {
            setJobFieldProperties(fields);
        }
        private void setJobFieldProperties(Collection<string> fields)
        {

            if (fields == null || fields.Contains("*"))
            {
                containsJobName = true;
                containsJobSetting = true;
                containsJobConfig = true;
                containsJobBuilds = true;
                containsJobReport = true;
                containsJobStatus = true;
                return;
            }

            foreach (string field in fields)
            {
                string effectiveField = field.ToLower();
                switch (effectiveField)
                {
                    case Constants.JobNameField:
                        containsJobName = true;
                        break;
                    case Constants.JobSettingField:
                        containsJobSetting = true;
                        break;
                    case Constants.JobConfigField:
                        containsJobConfig = true;
                        break;
                    case Constants.JobHistoryField:
                        containsJobBuilds = true;
                        break;
                    case Constants.JobStatusField:
                        containsJobStatus = true;
                        break;
                    case Constants.JobReportField:
                        containsJobReport = true;
                        break;
                }
            }
        }
    }
}
