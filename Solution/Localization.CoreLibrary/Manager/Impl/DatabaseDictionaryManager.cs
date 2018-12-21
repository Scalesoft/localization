using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DatabaseDictionaryManager : ManagerBase, IDatabaseDictionaryManager
    {
        private readonly IDatabaseDictionaryService m_dbDictionaryService;

        public DatabaseDictionaryManager(
            ILocalizationConfiguration configuration, IDatabaseDictionaryService dbDictionaryService, ILogger logger = null
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
