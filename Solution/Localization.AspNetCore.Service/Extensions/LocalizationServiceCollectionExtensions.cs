using Localization.CoreLibrary.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.AspNetCore.Service.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<IAutoLocalizationManager>(Localization.CoreLibrary.Localization.Translator);
            services.AddSingleton<IAutoDictionaryManager>(Localization.CoreLibrary.Localization.Dictionary);

            services.AddTransient<ILocalization, LocalizationService>();
            services.AddTransient<IDictionary, DictionaryService>();
        }
    }
}