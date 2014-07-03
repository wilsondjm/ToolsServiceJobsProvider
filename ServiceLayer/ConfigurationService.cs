﻿using RequestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ConfigurationService
    {
        ConfigurationClient configurationClient;

        public ConfigurationService()
        {
            configurationClient = new ConfigurationClient();
        }

        public bool addConfiguration(string projectName, string config)
        {
            return configurationClient.addConfiguration(projectName, config);
        }

        public bool updateConfiguration(string projectName, string config)
        {
            return configurationClient.updateConfiguration(projectName, config);
        }

        public bool deleteConfiguration(string projectName)
        {
            return configurationClient.deleConfiguration(projectName);
        }

        public string getConfiguration(string projectName)
        {
            return configurationClient.getConfiguration(projectName);
        }
    }
}
