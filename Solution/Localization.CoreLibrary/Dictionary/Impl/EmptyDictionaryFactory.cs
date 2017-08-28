namespace Localization.CoreLibrary.Dictionary.Impl
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