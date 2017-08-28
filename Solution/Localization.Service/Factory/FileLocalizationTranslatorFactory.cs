using Localization.CoreLibrary.Manager;

namespace Localization.Service.Factory
{
    public class FileLocalizationTranslatorFactory : ILocalizationTranslationFactory
    {
        public ILocalizationManager Create()
        {
            return Localization.CoreLibrary.Localization.FileTranslator;
        }
    }
}