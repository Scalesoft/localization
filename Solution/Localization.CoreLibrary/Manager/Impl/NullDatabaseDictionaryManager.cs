using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Manager.Impl
{
    public class NullDatabaseDictionaryManager : IDictionaryManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private const string NotSetMessage = "Database dictionary manager is not set.";
        
        private void LogNotSet()
        {
            if (Logger.IsInformationEnabled())
            {
                Logger.LogInformation(NotSetMessage);
            }
        }

        public Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return new Dictionary<string, LocalizedString>();
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return new Dictionary<string, PluralizedString>();
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return new Dictionary<string, LocalizedString>();
        }

        public CultureInfo DefaultCulture()
        {
            LogNotSet();
            return null;
        }

        public string DefaultScope()
        {
            LogNotSet();
            return null;
        }
    }
}