using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NHibernate;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Database.NHibernate.Mappings;
using Scalesoft.Localization.Database.NHibernate.Service;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate
{
    public class NHibernateDatabaseConfiguration : IDatabaseConfiguration
    {
        private readonly ISessionFactory m_sessionFactory;

        /// <summary>
        /// Configure Localization library to use NHibernate for database storage.
        ///  </summary>
        /// <param name="sessionFactory">Specify ISessionFactory if this factory should be registered to IoC. If container already contains ISessionFactory, this parameter should be null.</param>
        public NHibernateDatabaseConfiguration(ISessionFactory sessionFactory = null)
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
            services.AddSingleton<PluralizedStaticTextUoW>();
            services.AddSingleton<ConstantStaticTextUoW>();

            services.TryAddSingleton<IMemoryCache, MemoryCache>();
            services.Configure<MemoryCacheOptions>(options => { });

            if (m_sessionFactory != null)
            {
                services.TryAddSingleton(m_sessionFactory);
            }
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
