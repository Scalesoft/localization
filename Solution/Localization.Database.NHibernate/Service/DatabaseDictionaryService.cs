using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate.Service
{
    public class DatabaseDictionaryService : DatabaseServiceBase, IDatabaseDictionaryService
    {
        public DatabaseDictionaryService(
            LocalizationConfiguration configuration,
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