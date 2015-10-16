using System;
using System.Configuration;

namespace RSS.PubMed
{
    internal class PubMedConfiguration
    {
        public string BaseUrl => GetConfigSetting("PubMedBaseUrl");
        public string IdConverterBaseUrl => GetConfigSetting("PubMedBaseIdConverterUrl");
        public string FullArticleBaseUrl => GetConfigSetting("PubMedBaseFullArticleUrl");
        public string Database => GetConfigSetting("PubMedDatabase");
        public string ApplicationName => GetConfigSetting("PubMedApplicationName");
        public string Email => GetConfigSetting("PubMedDeveloperEmail");

        private string GetConfigSetting(string key)
        {
            if(ConfigurationManager.AppSettings[key] == null) throw new Exception("Configuration setting " + key + " does not exist");

            return ConfigurationManager.AppSettings[key];
        }
    }
}