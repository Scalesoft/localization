using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Entity;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Service
{
    public class DatabaseDynamicTextService : DatabaseServiceBase, IDatabaseDynamicTextService
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

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

            return new DynamicText
            {
                FallBack = value.Culture.Name != cultureInfo.Name,
                Culture = value.Culture.Name,
                DictionaryScope = value.DictionaryScope.Name,
                Format = value.Format,
                ModificationTime = value.ModificationTime,
                ModificationUser = value.ModificationUser,
                Name = value.Name,
                Text = value.Text
            };
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);
            StaticTextDao staticTextDao = new StaticTextDao(m_dbContext.StaticText);
            IList<StaticText> values = staticTextDao.FindByNameAndScope(name, dictionaryScope, m_dbContext.CultureHierarchy);

            var resultList = new List<DynamicText>();
            foreach (var value in values)
            {
                var dynamicText = new DynamicText
                {
                    FallBack = false,
                    Culture = value.Culture.Name,
                    DictionaryScope = value.DictionaryScope.Name,
                    Format = value.Format,
                    ModificationTime = value.ModificationTime,
                    ModificationUser = value.ModificationUser,
                    Name = value.Name,
                    Text = value.Text
                };
                resultList.Add(dynamicText);
            }
            
            return resultList;
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText)
        {
            StaticTextDao dao = new StaticTextDao(m_dbContext.StaticText);
            DictionaryScope dictionaryScope = GetDictionaryScope(dynamicText.DictionaryScope);
            if (dictionaryScope.Name != dynamicText.DictionaryScope)
            {
                dictionaryScope = CreateDictionaryScope(dynamicText.DictionaryScope);
            }          
            Culture culture = GetCulture(dynamicText.Culture);
            bool existsInCulture = culture.Name == dynamicText.Culture;

            if (!existsInCulture)
            {
                culture = CreateCulture(dynamicText.Culture);
                CreateCultureHierarchy(culture);
            }            

            StaticText staticText = dao.FindByNameAndCultureAndScope(dynamicText.Name, culture, dictionaryScope, m_dbContext.CultureHierarchy);
            if (staticText == null || !existsInCulture || staticText.CultureId != culture.Id)
            {
                staticText = new StaticText();
                staticText.Format = dynamicText.Format;
                staticText.ModificationTime = DateTime.UtcNow;
                staticText.ModificationUser = dynamicText.ModificationUser;
                staticText.Name = dynamicText.Name;
                staticText.Text = dynamicText.Text;
                staticText.Culture = culture;
                staticText.DictionaryScope = dictionaryScope;

                dao.Create(staticText);
            }
            else
            {
                staticText.Format = dynamicText.Format;
                staticText.ModificationTime = DateTime.UtcNow;
                staticText.ModificationUser = dynamicText.ModificationUser;
                staticText.Name = dynamicText.Name;
                staticText.Text = dynamicText.Text;

                dao.Update(staticText);
            }    

            DbContext dbContext = (DbContext) m_dbContext;
            dbContext.SaveChanges();

            return dynamicText;
        }

        private Culture CreateCulture(string cultureName)
        {
            CultureDao cultureDao = new CultureDao(m_dbContext.Culture);
            return cultureDao.Create(new Culture() { Id = 0, Name = cultureName });
        }

        private void CreateCultureHierarchy(Culture culture)
        {
            CultureHierarchyDao cultureHierarchyDao = new CultureHierarchyDao(m_dbContext.CultureHierarchy);
            CultureDao cultureDao = new CultureDao(m_dbContext.Culture);

            cultureHierarchyDao.MakeCultureSelfReferencing(culture);

            string defaultCultureName = Configuration.DefaultCulture().Name;
            Culture defaultCulture = cultureDao.FindByName(defaultCultureName);

            CultureInfo cultureInfo = new CultureInfo(culture.Name);
            if (cultureInfo.IsNeutralCulture) //Just reference to default culture
            {                
                cultureHierarchyDao.MakeCultureReference(culture, defaultCulture, 1);
            }
            else
            {
                string parentCultureName = cultureInfo.Parent.Name;
                Culture parentCulture = cultureDao.FindByName(parentCultureName);
                if (parentCulture == null)
                {
                    cultureHierarchyDao.MakeCultureReference(culture, defaultCulture, 1);
                }
                else
                {
                    cultureHierarchyDao.MakeCultureReference(culture, parentCulture, 1);
                    cultureHierarchyDao.MakeCultureReference(culture, defaultCulture, 2);
                }
            }
        }


        private DictionaryScope CreateDictionaryScope(string dictionaryScopeName)
        {
            DictionaryScopeDao dsDao = new DictionaryScopeDao(m_dbContext.DictionaryScope);
            return dsDao.Create(new DictionaryScope() { Name = dictionaryScopeName });
        }

    }
}