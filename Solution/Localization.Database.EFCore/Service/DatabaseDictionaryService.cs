using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Service
{
    public sealed class DatabaseDictionaryService : DatabaseServiceBase, IDatabaseDictionaryService
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public DatabaseDictionaryService(Func<IDatabaseStaticTextContext> dbContext, IConfiguration configuration)
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
