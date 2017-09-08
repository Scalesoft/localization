using System;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Util;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Microsoft.Extensions.Localization;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;

namespace Localization.Database.EFCore.Service
{
    public sealed class DatabaseTranslateService : DatabaseServiceBase, IDatabaseTranslateService
    {
        
        public DatabaseTranslateService(IDatabaseStaticTextContext dbContext, IConfiguration configuration) 
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
            //Should be empty.
        }

        public LocalizedString DatabaseTranslate(string text, CultureInfo cultureInfo, string scope)
        {
            Culture culture = GetCulture(cultureInfo.Name);
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);

            StaticTextDao staticTextDao = new StaticTextDao(DbContext.StaticText);
            IStaticText dbResult = staticTextDao.FindByNameAndCultureAndScope(text, culture, dictionaryScope);

            if (dbResult == null)
            {
                return null;
            }
            return new LocalizedString(text, dbResult.Text, false);
        }

        public LocalizedString DatabaseTranslateFormat(string text, object[] parameters, CultureInfo cultureInfo, string scope)
        {
            CultureDao cultureDao = new CultureDao(DbContext.Culture);
            Culture culture = cultureDao.FindByName(cultureInfo.Name);

            DictionaryScopeDao dictionaryScopeDao = new DictionaryScopeDao(DbContext.DictionaryScope);
            DictionaryScope dictionaryScope = dictionaryScopeDao.FindByName(scope);

            StaticTextDao staticTextDao = new StaticTextDao(DbContext.StaticText);
            IStaticText dbResult = staticTextDao.FindByNameAndCultureAndScope(text, culture, dictionaryScope);

            return new LocalizedString(text, dbResult.Text, false);
        }

        public LocalizedString DatabaseTranslatePluralization(string text, int number, CultureInfo cultureInfo, string scope)
        {
            throw new NotImplementedException();
        }

        public LocalizedString DatabaseTranslateConstant(string text, CultureInfo cultureInfo, string scope)
        {
            throw new NotImplementedException();
        }
    }
}