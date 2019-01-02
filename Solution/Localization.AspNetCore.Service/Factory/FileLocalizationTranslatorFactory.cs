using Localization.CoreLibrary.Manager;

namespace Localization.AspNetCore.Service.Factory
{
    public class FileLocalizationTranslatorFactory : ILocalizationTranslationFactory
    {
        public ILocalizationManager Create()
        {
            return CoreLibrary.Localization.FileTranslator;
        }
    }
}