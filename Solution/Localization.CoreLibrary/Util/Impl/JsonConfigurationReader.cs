using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Localization.CoreLibrary.Util.Impl
{
    internal class JsonConfigurationReader
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private readonly string m_configurationFilePath;

        public JsonConfigurationReader(string configurationFilePath)
        {
            CheckFileExists(configurationFilePath);

            m_configurationFilePath = configurationFilePath;
        }

        public IConfiguration ReadConfiguration()
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            LocalizationConfiguration.Configuration configuration;

            using (var stream = new FileStream(m_configurationFilePath, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                configuration = serializer.Deserialize<LocalizationConfiguration.Configuration>(jsonReader);
            }

            return new LocalizationConfiguration(configuration);
        }

        private void CheckFileExists(string configurationFilePath)
        {
            if (!File.Exists(configurationFilePath))
            {
                var errorMsg = string.Format("Configuration file \"{0}\" does not exist or you don't have permission to read.",
                    configurationFilePath);
                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(errorMsg);
                }

                throw new LibraryConfigurationException(errorMsg);
            }
        }
    }
}
