using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Dictionary.Factory;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Core.Resolver;

namespace Scalesoft.Localization.Core.IoC
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationCore(
            this IServiceCollection services, LocalizationConfiguration configuration, IDatabaseConfiguration databaseConfiguration
        )
        {
            services.AddSingleton(configuration);

            services.AddTransient<IDictionaryFactory, JsonDictionaryFactory>();

            services.AddTransient<IFileLocalizationManager, FileLocalizationManager>();
            services.AddTransient<IFileDictionaryManager, FileDictionaryManager>();

            services.AddTransient<IAutoDictionaryManager, DictionaryManager>();
            services.AddTransient<IAutoLocalizationManager, LocalizationManager>();

            services.AddTransient<FallbackCultureResolver>();

            if (databaseConfiguration != null)
            {
                databaseConfiguration.RegisterToIoc(services);
            }
            else
            {
                services.TryAddTransient<IDatabaseLocalizationManager, NullDatabaseLocalizationManager>();
                services.TryAddTransient<IDatabaseDictionaryManager, NullDatabaseDictionaryManager>();
                services.TryAddTransient<IDatabaseDynamicTextService, NullDatabaseDynamicTextService>();
            }
        }
    }
}