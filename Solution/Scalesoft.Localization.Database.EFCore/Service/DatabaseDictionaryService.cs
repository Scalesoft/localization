using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Database.EFCore.Dao.Impl;
using Scalesoft.Localization.Database.EFCore.Data;
using Scalesoft.Localization.Database.EFCore.Logging;

namespace Scalesoft.Localization.Database.EFCore.Service
{
    public sealed class DatabaseDictionaryService : DatabaseServiceBase, IDatabaseDictionaryService
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public DatabaseDictionaryService(Func<IDatabaseStaticTextContext> dbContext, LocalizationConfiguration configuration)
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
            //Should be empty.
        }

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);

                var staticTextDao = new StaticTextDao(dbContext.StaticText);
                var result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                var resultDictionary = new Dictionary<string, LocalizedString>();
                foreach (var singleStaticText in result)
                {
                    resultDictionary.Add(singleStaticText.Name, new LocalizedString(singleStaticText.Name, singleStaticText.Text, false));
                }

                return resultDictionary;
            }
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);

                var pluralizedStaticTextDao = new PluralizedStaticTextDao(dbContext.PluralizedStaticText);
                var result = pluralizedStaticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
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
        }

        public IDictionary<string, ClientPluralizedString> GetClientPluralizedDictionary(CultureInfo cultureInfo, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);

                var pluralizedStaticTextDao = new PluralizedStaticTextDao(dbContext.PluralizedStaticText);
                var result = pluralizedStaticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                var resultDictionary = new Dictionary<string, ClientPluralizedString>();
                foreach (var singleplPluralizedStaticText in result)
                {
                    var pluralizedString = new PluralizedString(new LocalizedString(singleplPluralizedStaticText.Name,
                        singleplPluralizedStaticText.Text, false));
                    foreach (var intervalText in singleplPluralizedStaticText.IntervalTexts)
                    {
                        pluralizedString.Add(new PluralizationInterval(intervalText.IntervalStart, intervalText.IntervalEnd)
                            , new LocalizedString(singleplPluralizedStaticText.Name, intervalText.Text));
                    }

                    resultDictionary.Add(singleplPluralizedStaticText.Name, new ClientPluralizedString(pluralizedString));
                }

                return resultDictionary;
            }
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);

                var constantStaticTextDao = new ConstantStaticTextDao(dbContext.ConstantStaticText);
                var result = constantStaticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
                var resultDictionary = new Dictionary<string, LocalizedString>();
                foreach (var singleConstantStaticText in result)
                {
                    resultDictionary.Add(singleConstantStaticText.Name, new LocalizedString(singleConstantStaticText.Name, singleConstantStaticText.Text, false));
                }

                return resultDictionary;
            }
        }
    }
}
