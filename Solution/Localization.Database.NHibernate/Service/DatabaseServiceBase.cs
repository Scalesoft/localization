using System;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Logging;
using Localization.Database.Abstractions.Entity;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Service
{
    public abstract class DatabaseServiceBase
    {
        private const int CacheTimeSpanInSeconds = 30;

        private readonly ILogger m_logger;
        protected readonly CultureUoW m_cultureUoW;
        protected readonly DictionaryScopeUoW m_dictionaryScopeUoW;
        protected readonly LocalizationConfiguration m_configuration;

        private readonly IMemoryCache m_memoryCache;

        protected DatabaseServiceBase(
            LocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            DictionaryScopeUoW dictionaryScopeUoW,
            ILogger<DatabaseServiceBase> logger,
            IMemoryCache memoryCache
        )
        {
            m_configuration = configuration;
            m_cultureUoW = cultureUoW;
            m_dictionaryScopeUoW = dictionaryScopeUoW;
            m_logger = logger;
            m_memoryCache = memoryCache;
        }

        public ICulture GetCachedCultureByNameOrGetDefault(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName)) throw new ArgumentException("Argument is required", nameof(cultureName));

            return m_memoryCache.GetOrCreate(
                cultureName,
                entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(CacheTimeSpanInSeconds);

                    return GetCultureByNameOrGetDefault(cultureName);
                }
            );
        }

        public ICulture GetCultureByNameOrGetDefault(string cultureName)
        {
            ICulture resultCulture = m_cultureUoW.GetCultureByName(cultureName);

            if (resultCulture == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(
                        $"Culture {cultureName} from library configuration is not in database.");
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

        public IDictionaryScope GetCachedDictionaryScope(string scopeName)
        {
            if (string.IsNullOrEmpty(scopeName)) throw new ArgumentException("Argument is required", nameof(scopeName));

            return m_memoryCache.GetOrCreate(
                scopeName,
                entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(CacheTimeSpanInSeconds);

                    return GetDictionaryScope(scopeName);
                }
            );
        }

        protected IDictionaryScope GetDictionaryScope(string scopeName)
        {
            var resultDictionaryScope = m_dictionaryScopeUoW.GetScopeByName(scopeName);

            if (resultDictionaryScope == null)
            {
                resultDictionaryScope = m_dictionaryScopeUoW.GetScopeByName(m_configuration.DefaultScope);
            }

            if (resultDictionaryScope == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(
                        @"Default dictionary scope ""{0}"" from library configuration is not in database.",
                        m_configuration.DefaultScope);
                }
            }

            return resultDictionaryScope;
        }
    }
}
