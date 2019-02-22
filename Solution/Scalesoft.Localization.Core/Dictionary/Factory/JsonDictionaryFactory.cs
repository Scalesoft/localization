using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Dictionary.Impl;

namespace Scalesoft.Localization.Core.Dictionary.Factory
{
    public class JsonDictionaryFactory : IDictionaryFactory
    {
        private readonly ILogger m_logger;
        public string FileExtension => FileExtensions.First();

        public IList<string> FileExtensions => new List<string>
        {
            JsonLocalizationDictionary.JsonExtension,
            JsonLocalizationDictionary.Json5Extension,
        };

        public JsonDictionaryFactory(ILogger logger = null)
        {
            m_logger = logger;
        }

        public ILocalizationDictionary CreateDictionary(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return new JsonLocalizationDictionary(fileStream, filePath, m_logger);
            }
        }

        public ILocalizationDictionary CreateDictionary(Stream resourceStream)
        {
            return new JsonLocalizationDictionary(resourceStream, m_logger);
        }

        public static IDictionaryFactory FactoryInstance => new JsonDictionaryFactory();
    }
}