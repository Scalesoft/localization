using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.Service;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Factory
{
    public class DatabaseServiceFactory : IDatabaseServiceFactory
    {
        private readonly CultureUoW m_cultureUoW;

        public DatabaseServiceFactory(
            CultureUoW cultureUoW
        )
        {
            m_cultureUoW = cultureUoW;
        }

        public IDatabaseTranslateService CreateTranslateService(
            ILocalizationConfiguration configuration, ILoggerFactory loggerFactory
        )
        {
            return new DatabaseTranslateService(
                loggerFactory.CreateLogger<DatabaseTranslateService>(),
                m_cultureUoW,
                configuration
            );
        }

        public IDatabaseDictionaryService CreateDictionaryService(
            ILocalizationConfiguration configuration, ILoggerFactory loggerFactory
        )
        {
            return new DatabaseDictionaryService(
                loggerFactory.CreateLogger<DatabaseDictionaryService>(),
                m_cultureUoW,
                configuration
            );
        }

        public IDatabaseDynamicTextService CreateDatabaseDynamicTextService(
            ILocalizationConfiguration configuration, ILoggerFactory loggerFactory
        )
        {
            return new DatabaseDynamicTextService(
                loggerFactory.CreateLogger<DatabaseDynamicTextService>(),
                m_cultureUoW,
                configuration
            );
        }
    }
}
