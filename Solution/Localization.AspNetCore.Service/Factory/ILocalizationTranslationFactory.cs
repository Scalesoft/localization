using Localization.CoreLibrary.Manager;

namespace Localization.AspNetCore.Service.Factory
{
    public interface ILocalizationTranslationFactory
    {
        ILocalizationManager Create();
    }
}