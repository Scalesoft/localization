using Localization.CoreLibrary.Manager;

namespace Localization.Service.Factory
{
    public interface ILocalizationTranslationFactory
    {
        ILocalizationManager Create();
    }
}