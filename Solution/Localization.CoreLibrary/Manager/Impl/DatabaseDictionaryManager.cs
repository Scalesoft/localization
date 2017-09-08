using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class DatabaseDictionaryManager : ManagerBase, IDictionaryManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly IDatabaseDictionaryService m_dbDictionaryService;

        public DatabaseDictionaryManager(IConfiguration configuration, IDatabaseDictionaryService dbDictionaryService)
            : base(configuration)
        {
            m_dbDictionaryService = dbDictionaryService;
        }

        public Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetDictionary(cultureInfo, scope);
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetPluralizedDictionary(cultureInfo, scope);
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
           cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            return m_dbDictionaryService.GetConstantsDictionary(cultureInfo, scope);
        }

        public CultureInfo DefaultCulture()
        {
            return Configuration.DefaultCulture();
        }

        public string DefaultScope()
        {
            return Localization.DefaultScope;
        }
    }
}