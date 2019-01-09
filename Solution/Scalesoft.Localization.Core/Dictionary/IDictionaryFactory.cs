using System.IO;

namespace Scalesoft.Localization.Core.Dictionary
{
    public interface IDictionaryFactory
    {
        /// <summary>
        /// Creates new instance of ILocalizationDictionary.
        /// </summary>
        /// <returns>New instance of ILocalizationDictionary</returns>
        ILocalizationDictionary CreateDictionary(Stream resourceStream);

        /// <summary>
        /// Creates new instance of ILocalizationDictionary.
        /// </summary>
        /// <returns>New instance of ILocalizationDictionary</returns>
        ILocalizationDictionary CreateDictionary(string filePath);

        /// <summary>
        /// Returns resource file extension.
        /// </summary>
        string FileExtension { get; }
    }
}
