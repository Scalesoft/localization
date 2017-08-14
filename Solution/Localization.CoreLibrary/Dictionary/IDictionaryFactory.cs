namespace Localization.CoreLibrary.Dictionary
{
    public interface IDictionaryFactory
    {
        /// <summary>
        /// Creates new instnace of ILocalizationDictionary.
        /// </summary>
        /// <returns>New instnace of ILocalizationDictionar</returns>
        ILocalizationDictionary CreateDictionary();

        /// <summary>
        /// Returns resource file extension.
        /// </summary>
        string FileExtension { get; }
    }
}