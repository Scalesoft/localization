using System;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Entity;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Service
{
    public class DatabaseDynamicTextService : DatabaseServiceBase, IDatabaseDynamicTextService
    {
        private readonly IDatabaseStaticTextContext m_dbContext;
        

        public DatabaseDynamicTextService(IDatabaseStaticTextContext dbContext, IConfiguration configuration)
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
            m_dbContext = dbContext;
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);
            Culture culture = GetCulture(cultureInfo.Name);
                
            StaticText value =
                new StaticTextDao(m_dbContext.StaticText).FindByNameAndCultureAndScope(name, culture, dictionaryScope, m_dbContext.CultureHierarchy);

            return new DynamicText()
            {
                Culture = value.Culture.Name,
                DictionaryScope = value.DictionaryScope.Name,
                Format = value.Format,
                ModificationTime = value.ModificationTime,
                ModificationUser = value.ModificationUser,
                Name = value.Name,
                Text = value.Text
            };
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText)
        {
            StaticTextDao dao = new StaticTextDao(m_dbContext.StaticText);
            DictionaryScope dictionaryScope = GetDictionaryScope(dynamicText.DictionaryScope);
            if (dictionaryScope == null)
            {
                DictionaryScopeDao dsDao = new DictionaryScopeDao(m_dbContext.DictionaryScope);
                dsDao.Create(new DictionaryScope(){Name = dynamicText.DictionaryScope});
            }          

            Culture culture = GetCulture(dynamicText.Culture);

            StaticText staticText = dao.FindByNameAndCultureAndScope(dynamicText.Name, culture, dictionaryScope, m_dbContext.CultureHierarchy);
            staticText.Format = dynamicText.Format;
            staticText.ModificationTime = DateTime.UtcNow;
            staticText.ModificationUser = dynamicText.ModificationUser;
            staticText.Name = dynamicText.Name;
            staticText.Text = dynamicText.Text;

            //StaticText staticText = new StaticText()
            //{
            //    //Culture = culture,
            //    //CultureId = culture.Id,
            //    //DictionaryScope = dictionaryScope,
            //    //DictionaryScopeId = dictionaryScope.Id,
            //    Format = dynamicText.Format,
            //    ModificationTime = DateTime.UtcNow,
            //    ModificationUser = dynamicText.ModificationUser,
            //    Name = dynamicText.Name,
            //    Text = dynamicText.Text
            //};


            dao.Update(staticText);

            DbContext dbContext = (DbContext) m_dbContext;
            dbContext.SaveChanges();

            return dynamicText;
        }
    }
}