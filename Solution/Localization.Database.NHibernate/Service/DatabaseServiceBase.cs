using System;
using Localization.CoreLibrary.Util;
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
        protected readonly CultureUoW CultureUoW;
        protected readonly DictionaryScopeUoW DictionaryScopeUoW;
        protected readonly ILocalizationConfiguration Configuration;

        private readonly IMemoryCache m_memoryCache;

        protected DatabaseServiceBase(
            ILocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            DictionaryScopeUoW dictionaryScopeUoW,
            ILogger logger,
            IMemoryCache memoryCache
        )
        {
            Configuration = configuration;
            CultureUoW = cultureUoW;
            DictionaryScopeUoW = dictionaryScopeUoW;
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
            ICulture resultCulture = CultureUoW.GetCultureByName(cultureName);

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
            var resultCulture = CultureUoW.GetCultureByName(Configuration.DefaultCulture.Name);

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
            var resultDictionaryScope = DictionaryScopeUoW.GetScopeByName(scopeName);

            if (resultDictionaryScope == null)
            {
                resultDictionaryScope = DictionaryScopeUoW.GetScopeByName(Configuration.DefaultScope);
            }

            if (resultDictionaryScope == null)
            {
                if (m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(
                        @"Default dictionary scope ""{0}"" from library configuration is not in database.",
                        Configuration.DefaultScope);
                }
            }

            return resultDictionaryScope;
        }
    }
}
