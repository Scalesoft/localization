using Localization.CoreLibrary.Dictionary.Impl;

namespace Localization.CoreLibrary.Dictionary.Factory
{
    public class EmptyDictionaryFactory : IDictionaryFactory
    {
        public ILocalizationDictionary CreateDictionary()
        {
            return new EmptyLocalizationDictionary();
        }

        public string FileExtension => EmptyLocalizationDictionary.EmptyExtension;
    }
}