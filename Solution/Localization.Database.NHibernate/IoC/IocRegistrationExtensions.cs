using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.Database.NHibernate.Provider;
using Localization.Database.NHibernate.Repository;
using Localization.Database.NHibernate.Service;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.Database.NHibernate.IoC
{
    public static class IocRegistrationExtensions
    {
        public static void RegisterLocalizationDataEntitiesComponents(this IServiceCollection services)
        {
            services.AddSingleton<CultureHierarchyRepository>();
            services.AddSingleton<CultureRepository>();

            services.AddSingleton<CultureHierarchyUoW>();
            services.AddSingleton<CultureUoW>();
        }

        public static void RegisterNHibernateLocalizationComponents(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseLocalizationManager, DatabaseLocalizationManager>();
            services.AddTransient<IDatabaseDictionaryManager, DatabaseDictionaryManager>();
            services.AddTransient<IDatabaseDynamicTextService, DatabaseDynamicTextService>();

            services.AddTransient<IDatabaseDictionaryService, DatabaseDictionaryService>();
            services.AddTransient<IDatabaseTranslateService, DatabaseTranslateService>();
            services.AddTransient<LocalizationMappingProvider>();
        }
    }
}
