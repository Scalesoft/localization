
using Localization.CoreLibrary.Dictionary.Impl;

namespace Localization.CoreLibrary.Dictionary.Factory
{
    public class JsonDictionaryFactory : IDictionaryFactory
    {
        string IDictionaryFactory.FileExtension => JsonLocalizationDictionary.JsonExtension;

        public ILocalizationDictionary CreateDictionary()
        {
            return new JsonLocalizationDictionary();
        }

        public static IDictionaryFactory FactoryInstance => new JsonDictionaryFactory();
    }
}