namespace Localization.CoreLibrary.Dictionary
{
    public interface IDictionaryFactory
    {
        /// <summary>
        /// Creates new instance of ILocalizationDictionary.
        /// </summary>
        /// <returns>New instance of ILocalizationDictionary</returns>
        ILocalizationDictionary CreateDictionary();

        /// <summary>
        /// Returns resource file extension.
        /// </summary>
        string FileExtension { get; }
    }
}