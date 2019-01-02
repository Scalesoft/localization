using System.IO;
using Localization.CoreLibrary.Dictionary.Impl;

namespace Localization.CoreLibrary.Dictionary.Factory
{
    public class EmptyDictionaryFactory : IDictionaryFactory
    {
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

        public string FileExtension => EmptyLocalizationDictionary.EmptyExtension;
    }
}
