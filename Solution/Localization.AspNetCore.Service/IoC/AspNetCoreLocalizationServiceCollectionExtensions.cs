using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.AspNetCore.Factory;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.AspNetCore.Service;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.IoC;

namespace Scalesoft.Localization.AspNetCore.IoC
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
