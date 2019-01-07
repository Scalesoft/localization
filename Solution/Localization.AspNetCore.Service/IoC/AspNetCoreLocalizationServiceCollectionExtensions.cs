using Localization.AspNetCore.Service.Factory;
using Localization.AspNetCore.Service.Manager;
using Localization.AspNetCore.Service.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service.IoC
{
    public static class AspNetCoreLocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();
            services.AddSingleton<RequestCultureManager>();

            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddTransient<IDynamicTextService, DynamicTextService>();
        }
    }
}
