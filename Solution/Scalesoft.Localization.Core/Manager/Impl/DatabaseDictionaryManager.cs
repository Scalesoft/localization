using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Pluralization;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class DatabaseDictionaryManager : ManagerBase, IDatabaseDictionaryManager
    {
        private readonly IDatabaseDictionaryService m_dbDictionaryService;

        public DatabaseDictionaryManager(
            LocalizationConfiguration configuration, IDatabaseDictionaryService dbDictionaryService, ILogger<DatabaseDictionaryManager> logger = null
        ) : base(configuration, logger)
        {
            m_dbDictionaryService = dbDictionaryService;
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetDictionary(cultureInfo, scope);
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetPluralizedDictionary(cultureInfo, scope);
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetConstantsDictionary(cultureInfo, scope);
        }
    }
}
