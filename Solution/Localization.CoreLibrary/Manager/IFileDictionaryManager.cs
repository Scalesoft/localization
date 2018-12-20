using System.Globalization;
using Localization.CoreLibrary.Dictionary;

namespace Localization.CoreLibrary.Manager
{
    public interface IFileDictionaryManager : IDictionaryManager
    {
        void AddDictionaryToHierarchyTrees(ILocalizationDictionary dictionary);

        ILocalizationDictionary GetLocalizationDictionary(CultureInfo cultureInfo = null, string scope = null);
    }
}
