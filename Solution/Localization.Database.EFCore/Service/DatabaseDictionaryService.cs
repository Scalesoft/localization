using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Localization.Database.Abstractions.Entity;
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

        public DatabaseDictionaryService(IDatabaseStaticTextContext dbContext, IConfiguration configuration)
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
            //Should be empty.
        }

        public Dictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo, string scope)
        {
            Culture culture = GetCulture(cultureInfo.Name);
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);

            StaticTextDao staticTextDao = new StaticTextDao(DbContext.StaticText);
            StaticText[] result = staticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
            Dictionary<string, LocalizedString> resultDictionary = new Dictionary<string, LocalizedString>();
            foreach (StaticText singleStaticText in result)
            {
                resultDictionary.Add(singleStaticText.Name, new LocalizedString(singleStaticText.Name, singleStaticText.Text, false));
            }

            return resultDictionary;
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo, string scope)
        {
            Culture culture = GetCulture(cultureInfo.Name);
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);

            PluralizedStaticTextDao pluralizedStaticTextDao = new PluralizedStaticTextDao(DbContext.PluralizedStaticText);
            PluralizedStaticText[] result = pluralizedStaticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
            Dictionary<string, PluralizedString> resultDictionary = new Dictionary<string, PluralizedString>();
            foreach (PluralizedStaticText singleplPluralizedStaticText in result)
            {
                PluralizedString pluralizedString = new PluralizedString(new LocalizedString(singleplPluralizedStaticText.Name,
                    singleplPluralizedStaticText.Text, false));
                foreach (IntervalText intervalText in singleplPluralizedStaticText.IntervalTexts)
                {
                    pluralizedString.Add(new PluralizationInterval(intervalText.IntervalStart, intervalText.IntervalEnd)
                        , new LocalizedString(singleplPluralizedStaticText.Name, intervalText.Text));
                }

                resultDictionary.Add(singleplPluralizedStaticText.Name, pluralizedString);
            }

            return resultDictionary;
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo, string scope)
        {
            Culture culture = GetCulture(cultureInfo.Name);
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);

            ConstantStaticTextDao constantStaticTextDao = new ConstantStaticTextDao(DbContext.ConstantStaticText);
            ConstantStaticText[] result = constantStaticTextDao.FindAllByCultureAndScope(culture, dictionaryScope);
            Dictionary<string, LocalizedString> resultDictionary = new Dictionary<string, LocalizedString>();
            foreach (ConstantStaticText singleConstantStaticText in result)
            {
                resultDictionary.Add(singleConstantStaticText.Name, new LocalizedString(singleConstantStaticText.Name, singleConstantStaticText.Text, false));
            }

            return resultDictionary;
        }
    }
}