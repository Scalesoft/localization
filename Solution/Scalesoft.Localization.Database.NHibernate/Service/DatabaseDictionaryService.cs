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
        private readonly StaticTextUoW m_staticTextUoW;
        private readonly PluralizedStaticTextUoW m_pluralizedStaticTextUoW;
        private readonly ConstantStaticTextUoW m_constantStaticTextUoW;

        public DatabaseDictionaryService(
            LocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            StaticTextUoW staticTextUoW,
            PluralizedStaticTextUoW pluralizedStaticTextUoW,
            ConstantStaticTextUoW constantStaticTextUoW,
            DictionaryScopeUoW dictionaryScopeUoW,
            ILogger<DatabaseDictionaryService> logger,
            IMemoryCache memoryCache
        ) : base(configuration, cultureUoW, dictionaryScopeUoW, logger, memoryCache)
        {
            m_staticTextUoW = staticTextUoW;
            m_pluralizedStaticTextUoW = pluralizedStaticTextUoW;
            m_constantStaticTextUoW = constantStaticTextUoW;
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope)
        {
            var culture = GetCultureByNameOrGetDefault(cultureInfo.Name);
            var dictionaryScope = GetDictionaryScope(scope);
            var result = m_staticTextUoW.FindAllByCultureAndScope(culture.Name, dictionaryScope.Name);
            var resultDictionary = new Dictionary<string, LocalizedString>();
            foreach (var singleStaticText in result)
            {
                resultDictionary.Add(singleStaticText.Name, new LocalizedString(singleStaticText.Name, singleStaticText.Text, false));
            }

            return resultDictionary;
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope)
        {
            var culture = GetCultureByNameOrGetDefault(cultureInfo.Name);
            var dictionaryScope = GetDictionaryScope(scope);

            var result = m_pluralizedStaticTextUoW.FindAllByCultureAndScope(culture.Name, dictionaryScope.Name);
            var resultDictionary = new Dictionary<string, PluralizedString>();
            foreach (var singleplPluralizedStaticText in result)
            {
                var pluralizedString = new PluralizedString(new LocalizedString(singleplPluralizedStaticText.Name,
                    singleplPluralizedStaticText.Text, false));
                foreach (var intervalText in singleplPluralizedStaticText.IntervalTexts)
                {
                    pluralizedString.Add(new PluralizationInterval(intervalText.IntervalStart, intervalText.IntervalEnd)
                        , new LocalizedString(singleplPluralizedStaticText.Name, intervalText.Text));
                }

                resultDictionary.Add(singleplPluralizedStaticText.Name, pluralizedString);
            }

            return resultDictionary;
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope)
        {
                var culture = GetCultureByNameOrGetDefault(cultureInfo.Name);
                var dictionaryScope = GetDictionaryScope(scope);

                var result = m_constantStaticTextUoW.FindAllByCultureAndScope(culture.Name, dictionaryScope.Name);
                var resultDictionary = new Dictionary<string, LocalizedString>();
                foreach (var singleConstantStaticText in result)
                {
                    resultDictionary.Add(singleConstantStaticText.Name, new LocalizedString(singleConstantStaticText.Name, singleConstantStaticText.Text, false));
                }

                return resultDictionary;
            
        }
    }
}