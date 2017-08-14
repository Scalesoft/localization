
namespace Localization.CoreLibrary.Dictionary.Impl
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