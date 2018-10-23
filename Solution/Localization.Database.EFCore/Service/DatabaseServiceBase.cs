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
        protected readonly IConfiguration m_configuration;

        protected DatabaseServiceBase(ILogger logger, Func<IDatabaseStaticTextContext> dbContext, IConfiguration configuration)
        {
            m_logger = logger;
            m_dbContextFunc = dbContext;
            m_configuration = configuration;
        }

        protected Culture GetCulture(IDatabaseStaticTextContext dbContext, string cultureName)
        {
            CultureDao cultureDao = new CultureDao(dbContext.Culture);

            Culture resultCulture = cultureDao.FindByName(cultureName);
            if (resultCulture == null)
            {
                resultCulture = cultureDao.FindByName(m_configuration.DefaultCulture().Name);
            }
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
            DictionaryScopeDao dictionaryScopeDao = new DictionaryScopeDao(dbContext.DictionaryScope);

            DictionaryScope resultDictionaryScope = dictionaryScopeDao.FindByName(scopeName);
            if (resultDictionaryScope == null)
            {
                resultDictionaryScope = dictionaryScopeDao.FindByName(CoreLibrary.Localization.DefaultScope);
            }
            if (resultDictionaryScope == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(@"Default dictionary scope ""{0}"" from library configuration is not in database.", CoreLibrary.Localization.DefaultScope);
                }
            }

            return resultDictionaryScope;
        }
    }
}