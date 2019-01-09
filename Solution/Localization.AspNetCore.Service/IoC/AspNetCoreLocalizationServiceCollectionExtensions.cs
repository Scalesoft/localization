using Localization.AspNetCore.Service.Factory;
using Localization.AspNetCore.Service.Manager;
using Localization.AspNetCore.Service.Service;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service.IoC
{
    public static class AspNetCoreLocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationService(this IServiceCollection services, LocalizationConfiguration configuration,
            IDatabaseConfiguration databaseConfiguration = null)
        {
            services.AddLocalizationCore(configuration, databaseConfiguration);

            services.AddSingleton<IStringLocalizerFactory, AttributeStringLocalizerFactory>();
            services.AddSingleton<RequestCultureManager>();

            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddTransient<IDynamicTextService, DynamicTextService>();
        }
    }
}
