using Localization.CoreLibrary.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.Service.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<ILocalizationManager>(Localization.CoreLibrary.Localization.FileTranslator);
            services.AddTransient<ILocalization, TransientLocalizationService>();
        }
    }
}