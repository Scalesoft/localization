using System;
using System.Collections.Generic;
using System.Linq;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.Database.NHibernate.Mappings;
using Localization.Database.NHibernate.Service;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NHibernate;

namespace Localization.Database.NHibernate
{
    public class NHibernateDatabaseConfiguration : IDatabaseConfiguration
    {
        private readonly ISessionFactory m_sessionFactory;

        public NHibernateDatabaseConfiguration(ISessionFactory sessionFactory)
        {
            m_sessionFactory = sessionFactory;
        }

        public void RegisterToIoc(IServiceCollection services)
        {
            services.AddTransient<IDatabaseLocalizationManager, DatabaseLocalizationManager>();
            services.AddTransient<IDatabaseDictionaryManager, DatabaseDictionaryManager>();
            services.AddTransient<IDatabaseDynamicTextService, DatabaseDynamicTextService>();

            services.AddTransient<IDatabaseDictionaryService, DatabaseDictionaryService>();
            services.AddTransient<IDatabaseTranslateService, DatabaseTranslateService>();

            services.AddSingleton<CultureHierarchyUoW>();
            services.AddSingleton<CultureUoW>();
            services.AddSingleton<DictionaryScopeUoW>();
            services.AddSingleton<StaticTextUoW>();

            services.TryAddSingleton(m_sessionFactory);
        }

        public static IEnumerable<Type> GetMappings()
        {
            var baseType = typeof(IMapping);
            var assembly = baseType.Assembly;

            return assembly.GetExportedTypes().Where(
                t => baseType.IsAssignableFrom(t)
            );
        }
    }
}
