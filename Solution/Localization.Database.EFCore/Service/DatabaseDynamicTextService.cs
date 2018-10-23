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
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Service
{
    public class DatabaseDynamicTextService : DatabaseServiceBase, IDatabaseDynamicTextService
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        

        public DatabaseDynamicTextService(Func<IDatabaseStaticTextContext> dbContext, IConfiguration configuration)
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                DictionaryScope dictionaryScope = GetDictionaryScope(dbContext, scope);
                Culture culture = GetCulture(dbContext, cultureInfo.Name);

                StaticText value =
                    new StaticTextDao(dbContext.StaticText).FindByNameAndCultureAndScope(name, culture, dictionaryScope, dbContext.CultureHierarchy);

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
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                DictionaryScope dictionaryScope = GetDictionaryScope(dbContext, scope);
                StaticTextDao staticTextDao = new StaticTextDao(dbContext.StaticText);
                IList<StaticText> values = staticTextDao.FindByNameAndScope(name, dictionaryScope, dbContext.CultureHierarchy);

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
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                StaticTextDao dao = new StaticTextDao(dbContext.StaticText);
                DictionaryScope dictionaryScope = GetDictionaryScope(dbContext, dynamicText.DictionaryScope);
                if (dictionaryScope.Name != dynamicText.DictionaryScope)
                {
                    dictionaryScope = CreateDictionaryScope(dbContext, dynamicText.DictionaryScope);
                }
                Culture culture = GetCulture(dbContext, dynamicText.Culture);
                bool existsInCulture = culture.Name == dynamicText.Culture;

                if (!existsInCulture)
                {
                    culture = CreateCulture(dbContext, dynamicText.Culture);
                    CreateCultureHierarchy(dbContext, culture);
                }

                StaticText staticText = dao.FindByNameAndCultureAndScope(dynamicText.Name, culture, dictionaryScope, dbContext.CultureHierarchy);
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

                dbContext.SaveChanges();

                return dynamicText;
            }
        }

        private Culture CreateCulture(IDatabaseStaticTextContext dbContext, string cultureName)
        {
            CultureDao cultureDao = new CultureDao(dbContext.Culture);
            return cultureDao.Create(new Culture() { Id = 0, Name = cultureName });
        }

        private void CreateCultureHierarchy(IDatabaseStaticTextContext dbContext, Culture culture)
        {
            CultureHierarchyDao cultureHierarchyDao = new CultureHierarchyDao(dbContext.CultureHierarchy);
            CultureDao cultureDao = new CultureDao(dbContext.Culture);

            cultureHierarchyDao.MakeCultureSelfReferencing(culture);

            string defaultCultureName = m_configuration.DefaultCulture().Name;
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


        private DictionaryScope CreateDictionaryScope(IDatabaseStaticTextContext dbContext, string dictionaryScopeName)
        {
            DictionaryScopeDao dsDao = new DictionaryScopeDao(dbContext.DictionaryScope);
            return dsDao.Create(new DictionaryScope() { Name = dictionaryScopeName });
        }

    }
}