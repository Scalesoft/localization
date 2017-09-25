using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Logging;
using Localization.Database.EFCore.Service;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Factory
{
    public class DatabaseServiceFactory : IDatabaseServiceFactory
    {
        private readonly IDatabaseStaticTextContext m_databaseStaticTextContext;

        public DatabaseServiceFactory(IDatabaseStaticTextContext databaseStaticTextContext)
        {
            m_databaseStaticTextContext = databaseStaticTextContext;
        }

        public IDatabaseTranslateService CreateTranslateService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);

            return new DatabaseTranslateService(m_databaseStaticTextContext, configuration);
        }

        public IDatabaseDictionaryService CreateDictionaryService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);

            return new DatabaseDictionaryService(m_databaseStaticTextContext, configuration);
        }

        public IDatabaseDynamicTextService CreateDatabaseDynamicTextService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);

            return new DatabaseDynamicTextService(m_databaseStaticTextContext, configuration);
        }
    }
}