using System.IO;
using Localization.CoreLibrary.Dictionary.Impl;

namespace Localization.CoreLibrary.Dictionary.Factory
{
    public class JsonDictionaryFactory : IDictionaryFactory
    {
        string IDictionaryFactory.FileExtension => JsonLocalizationDictionary.JsonExtension;

        public ILocalizationDictionary CreateDictionary(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return new JsonLocalizationDictionary(fileStream, filePath);
            }
        }
        public ILocalizationDictionary CreateDictionary(Stream resourceStream)
        {
            return new JsonLocalizationDictionary(resourceStream);
        }

        public static IDictionaryFactory FactoryInstance => new JsonDictionaryFactory();
    }
}
