using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Resolver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Localization.CoreLibrary.IoC
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static void AddLocalizationCore(
            this IServiceCollection services
        )
        {
            services.AddTransient<IDictionaryFactory, JsonDictionaryFactory>();

            services.AddTransient<IFileLocalizationManager, FileLocalizationManager>();
            services.AddTransient<IFileDictionaryManager, FileDictionaryManager>();

            services.TryAddTransient<IDatabaseLocalizationManager, NullDatabaseLocalizationManager>();
            services.TryAddTransient<IDatabaseDictionaryManager, NullDatabaseDictionaryManager>();
            services.TryAddTransient<IDatabaseDynamicTextService, NullDatabaseDynamicTextService>();

            services.AddTransient<IAutoDictionaryManager, DictionaryManager>();
            services.AddTransient<IAutoLocalizationManager, LocalizationManager>();

            services.AddTransient<FallbackCultureResolver>();
        }
    }
}