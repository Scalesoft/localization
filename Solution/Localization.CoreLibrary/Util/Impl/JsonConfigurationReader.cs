using System.IO;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Util.Impl
{
    public class JsonConfigurationReader
    {
        private readonly ILogger m_logger;
        private readonly string m_configurationFilePath;

        public JsonConfigurationReader(string configurationFilePath, ILogger<JsonConfigurationReader> logger)
        {
            CheckFileExists(configurationFilePath);

            m_configurationFilePath = configurationFilePath;
            m_logger = logger;
        }

        public LocalizationConfiguration ReadConfiguration()
        {
            LocalizationConfiguration configuration = new LocalizationConfiguration();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(m_configurationFilePath, optional: false);

            configurationBuilder.Build()
                .Bind(configuration);

            return configuration;
        }

        private void CheckFileExists(string configurationFilePath)
        {
            if (File.Exists(configurationFilePath))
            {
                return;
            }

            var errorMsg = $"Configuration file \"{configurationFilePath}\" does not exist or you don't have permission to read.";

            if (m_logger.IsErrorEnabled())
            {
                m_logger.LogError(errorMsg);
            }

            throw new LibraryConfigurationException(errorMsg);
        }
    }
}
