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
        protected readonly IDatabaseStaticTextContext DbContext;
        protected readonly IConfiguration Configuration;

        protected DatabaseServiceBase(ILogger logger, IDatabaseStaticTextContext dbContext, IConfiguration configuration)
        {
            m_logger = logger;
            DbContext = dbContext;
            Configuration = configuration;
        }

        protected Culture GetCulture(string cultureName)
        {
            CultureDao cultureDao = new CultureDao(DbContext.Culture);

            Culture resultCulture = cultureDao.FindByName(cultureName);
            if (resultCulture == null)
            {
                resultCulture = cultureDao.FindByName(Configuration.DefaultCulture().Name);
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

        protected DictionaryScope GetDictionaryScope(string scopeName)
        {
            DictionaryScopeDao dictionaryScopeDao = new DictionaryScopeDao(DbContext.DictionaryScope);

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