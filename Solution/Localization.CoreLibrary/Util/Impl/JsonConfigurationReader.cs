using System.IO;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Localization.CoreLibrary.Util.Impl
{
    internal class JsonConfigurationReader
    {
        private readonly ILogger m_logger;
        private readonly string m_configurationFilePath;

        public JsonConfigurationReader(string configurationFilePath, ILogger<JsonConfigurationReader> logger = null)
        {
            CheckFileExists(configurationFilePath);

            m_configurationFilePath = configurationFilePath;
            m_logger = logger;
        }

        public ILocalizationConfiguration ReadConfiguration()
        {
            ILocalizationConfiguration configuration = new LocalizationConfiguration();

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

            var errorMsg = string.Format("Configuration file \"{0}\" does not exist or you don't have permission to read.",
                configurationFilePath);

            if (m_logger != null && m_logger.IsErrorEnabled())
            {
                m_logger.LogError(errorMsg);
            }

            throw new LibraryConfigurationException(errorMsg);
        }
    }
}
