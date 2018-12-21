using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Logging;
using Localization.Database.Abstractions.Entity;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Service
{
    public abstract class DatabaseServiceBase
    {
        private readonly ILogger m_logger;
        protected readonly CultureUoW m_cultureUoW;
        protected readonly ILocalizationConfiguration m_configuration;

        protected DatabaseServiceBase(
            ILocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            ILogger logger
        )
        {
            m_configuration = configuration;
            m_cultureUoW = cultureUoW;
            m_logger = logger;
        }

        public ICulture GetCultureByNameOrGetDefault(string cultureName)
        {
            ICulture resultCulture = m_cultureUoW.GetCultureByName(cultureName);

            if (resultCulture == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(
                        $"Culture {cultureName} and default culture from library configuration is not in database.");
                }

                resultCulture = GetDefaultCulture();
            }

            return resultCulture;
        }

        public ICulture GetDefaultCulture()
        {
            var resultCulture = m_cultureUoW.GetCultureByName(m_configuration.DefaultCulture.Name);

            if (resultCulture == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError("Default culture from library configuration is not in database.");
                }
            }

            return resultCulture;
        }

        /*protected DictionaryScope GetDictionaryScope(IDatabaseStaticTextContext dbContext, string scopeName)
        {
            var dictionaryScopeDao = new DictionaryScopeDao(dbContext.DictionaryScope);

            var resultDictionaryScope = dictionaryScopeDao.FindByName(scopeName);
            if (resultDictionaryScope == null)
            {
                resultDictionaryScope = dictionaryScopeDao.FindByName(CoreLibrary.Localization.DefaultScope);
            }

            if (resultDictionaryScope == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(
                        @"Default dictionary scope ""{0}"" from library configuration is not in database.",
                        CoreLibrary.Localization.DefaultScope);
                }
            }

            return resultDictionaryScope;
        }*/
    }
}
