using Localization.CoreLibrary.Dictionary;

namespace Localization.CoreLibrary.Manager
{
    public interface IFileDictionaryManager : IDictionaryManager
    {
        void AddDictionaryToHierarchyTrees(ILocalizationDictionary dictionary);
    }
}
