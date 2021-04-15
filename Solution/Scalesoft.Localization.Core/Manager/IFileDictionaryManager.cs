using System.Globalization;
using Scalesoft.Localization.Core.Dictionary;

namespace Scalesoft.Localization.Core.Manager
{
    public interface IFileDictionaryManager : IDictionaryManager
    {
        void AddDictionaryToHierarchyTrees(ILocalizationDictionary dictionary);

        ILocalizationDictionary GetLocalizationDictionary(CultureInfo cultureInfo = null, string scope = null);

        bool IsCultureSupported(CultureInfo cultureInfo);

        void AutoLoadDictionaries(IDictionaryFactory dictionaryFactory);
    }
}
