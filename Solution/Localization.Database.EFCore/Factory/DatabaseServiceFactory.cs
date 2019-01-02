using System;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Data.Impl;
using Localization.Database.EFCore.Logging;
using Localization.Database.EFCore.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Factory
{
    public class DatabaseServiceFactory : IDatabaseServiceFactory
    {
        private readonly DbContextOptions<StaticTextsContext> m_options;

        public DatabaseServiceFactory(Action<DbContextOptionsBuilder<StaticTextsContext>> options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StaticTextsContext>();
            options.Invoke(optionsBuilder);
            m_options = optionsBuilder.Options;
        }

        private StaticTextsContext CreateNewDbContext()
        {
            return new StaticTextsContext(m_options);
        }
        
        public IDatabaseTranslateService CreateTranslateService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);

            return new DatabaseTranslateService(CreateNewDbContext, configuration);
        }
        
        public IDatabaseDictionaryService CreateDictionaryService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);

            return new DatabaseDictionaryService(CreateNewDbContext, configuration);
        }

        public IDatabaseDynamicTextService CreateDatabaseDynamicTextService(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            LogProvider.AttachLoggerFactory(loggerFactory);

            return new DatabaseDynamicTextService(CreateNewDbContext, configuration);
        }
    }
}