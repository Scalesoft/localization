using Localization.AspNetCore.Service.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();

            services.AddTransient<ILocalization, LocalizationService>();
            services.AddTransient<IDictionary, DictionaryService>();
            services.AddTransient<IDynamicText, DynamicText>();
        }
    }
}