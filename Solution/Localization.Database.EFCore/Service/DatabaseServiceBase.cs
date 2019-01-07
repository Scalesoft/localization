using System;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Service
{
    public abstract class DatabaseServiceBase
    {
        private readonly ILogger m_logger;
        protected readonly Func<IDatabaseStaticTextContext> m_dbContextFunc;
        protected readonly ILocalizationConfiguration m_configuration;

        protected DatabaseServiceBase(ILogger logger, Func<IDatabaseStaticTextContext> dbContext, ILocalizationConfiguration configuration)
        {
            m_logger = logger;
            m_dbContextFunc = dbContext;
            m_configuration = configuration;
        }

        protected Culture GetCultureByNameOrGetDefault(IDatabaseStaticTextContext dbContext, string cultureName)
        {
            var cultureDao = new CultureDao(dbContext.Culture);

            var resultCulture = cultureDao.FindByName(cultureName) ?? cultureDao.FindByName(m_configuration.DefaultCulture.Name);

            if (resultCulture == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError($"Culture {cultureName} and default culture from library configuration is not in database.");
                }
            }

            return resultCulture;
        }

        protected Culture GetDefaultCulture(IDatabaseStaticTextContext dbContext)
        {
            var cultureDao = new CultureDao(dbContext.Culture);

            var resultCulture = cultureDao.FindByName(m_configuration.DefaultCulture.Name);

            if (resultCulture == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError("Default culture from library configuration is not in database.");
                }
            }

            return resultCulture;
        }

        protected DictionaryScope GetDictionaryScope(IDatabaseStaticTextContext dbContext, string scopeName)
        {
            var dictionaryScopeDao = new DictionaryScopeDao(dbContext.DictionaryScope);

            var resultDictionaryScope = dictionaryScopeDao.FindByName(scopeName);

            if (resultDictionaryScope == null)
            {
                resultDictionaryScope = dictionaryScopeDao.FindByName(m_configuration.DefaultScope);
            }
            if (resultDictionaryScope == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(@"Default dictionary scope ""{0}"" from library configuration is not in database.", m_configuration.DefaultScope);
                }
            }

            return resultDictionaryScope;
        }
    }
}
