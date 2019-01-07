using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Service
{
    public class DatabaseDictionaryService : DatabaseServiceBase, IDatabaseDictionaryService
    {
        public DatabaseDictionaryService(
            ILocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            DictionaryScopeUoW dictionaryScopeUoW,
            ILogger<DatabaseDictionaryService> logger,
            IMemoryCache memoryCache
        ) : base(configuration, cultureUoW, dictionaryScopeUoW, logger, memoryCache)
        {
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope)
        {
            throw new System.NotImplementedException();
        }
    }
}