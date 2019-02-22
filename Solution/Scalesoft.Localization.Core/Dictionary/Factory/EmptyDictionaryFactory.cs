using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Dictionary.Impl;

namespace Scalesoft.Localization.Core.Dictionary.Factory
{
    public class EmptyDictionaryFactory : IDictionaryFactory
    {
        private readonly ILogger m_logger;

        public EmptyDictionaryFactory(ILogger logger = null)
        {
            m_logger = logger;
        }

        public ILocalizationDictionary CreateDictionary(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return new EmptyLocalizationDictionary(fileStream, filePath);
            }
        }

        public ILocalizationDictionary CreateDictionary(Stream resourceStream)
        {
            return new EmptyLocalizationDictionary(resourceStream);
        }

        public string FileExtension => FileExtensions.First();

        public IList<string> FileExtensions => new List<string>
        {
            EmptyLocalizationDictionary.EmptyExtension
        };
    }
}