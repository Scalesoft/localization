using System;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Util.Impl
{
    public class JsonConfigurationReader
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private readonly string m_configPath;

        public JsonConfigurationReader(string configPath)
        {
            m_configPath = configPath;
        }    

        public string ReadSetting(string key)
        {
            IConfigurationRoot configuration = null;
            try
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile(m_configPath)
                    .Build();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            if (configuration == null)
            {
                Logger.LogError("Configuration file is null");
                return "";
            }         

            return configuration[key];
        }
    }
}