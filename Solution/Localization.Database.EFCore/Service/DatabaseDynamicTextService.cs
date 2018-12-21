using System;
using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Entity;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;

namespace Localization.Database.EFCore.Service
{
    public class DatabaseDynamicTextService : DatabaseServiceBase, IDatabaseDynamicTextService
    {
        public DatabaseDynamicTextService(Func<IDatabaseStaticTextContext> dbContext, ILocalizationConfiguration configuration)
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var dictionaryScope = GetDictionaryScope(dbContext, scope);
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);

                var value = new StaticTextDao(dbContext.StaticText).FindByNameAndCultureAndScope(
                    name, culture, dictionaryScope, dbContext.CultureHierarchy
                );

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
                var dictionaryScope = GetDictionaryScope(dbContext, scope);
                var staticTextDao = new StaticTextDao(dbContext.StaticText);
                var values = staticTextDao.FindByNameAndScope(name, dictionaryScope, dbContext.CultureHierarchy);

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

        public DynamicText SaveDynamicText(DynamicText dynamicText,
            IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var dao = new StaticTextDao(dbContext.StaticText);
                var dictionaryScope = GetDictionaryScope(dbContext, dynamicText.DictionaryScope);
                if (dictionaryScope.Name != dynamicText.DictionaryScope)
                {
                    dictionaryScope = CreateDictionaryScope(dbContext, dynamicText.DictionaryScope);
                }

                var culture = GetCultureByNameOrGetDefault(dbContext, dynamicText.Culture);
                var existsInCulture = culture.Name == dynamicText.Culture;

                if (!existsInCulture)
                {
                    culture = CreateCulture(dbContext, dynamicText.Culture);
                    CreateCultureHierarchy(dbContext, culture);
                }

                var staticText = dao.FindByNameAndCultureAndScope(
                    dynamicText.Name, culture, dictionaryScope, dbContext.CultureHierarchy
                );
                if (staticText == null || !existsInCulture || staticText.CultureId != culture.Id)
                {
                    staticText = new StaticText
                    {
                        Format = dynamicText.Format,
                        ModificationTime = DateTime.UtcNow,
                        ModificationUser = dynamicText.ModificationUser,
                        Name = dynamicText.Name,
                        Text = dynamicText.Text,
                        Culture = culture,
                        DictionaryScope = dictionaryScope
                    };

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

                ExecuteDefaultCultureAction(actionForDefaultCulture, dynamicText, culture, dictionaryScope, dbContext,
                    dao);

                dbContext.SaveChanges();

                return dynamicText;
            }
        }

        /// <summary>
        /// Executes an action on default culture text if it does not exist. Actions include - nothing, create empty string, copy current culture text
        /// </summary>
        /// <param name="actionForDefaultCulture">Specific action</param>
        /// <param name="dynamicText">Current dynamic text entity</param>
        /// <param name="currentCulture">Current culture entity</param>
        /// <param name="dictionaryScope">Current dictionary scope entity</param>
        /// <param name="dbContext">Database context</param>
        /// <param name="dao">DAO of static text entity</param>
        private void ExecuteDefaultCultureAction(IfDefaultNotExistAction actionForDefaultCulture,
            DynamicText dynamicText, Culture currentCulture,
            DictionaryScope dictionaryScope, IDatabaseStaticTextContext dbContext, StaticTextDao dao)
        {
            var defaultCulture = GetDefaultCulture(dbContext);
            if (currentCulture == defaultCulture)
            {
                return;
            }

            var defaultText = dao.FindByNameAndCultureAndScope(
                dynamicText.Name, defaultCulture, dictionaryScope, dbContext.CultureHierarchy
            );

            if (defaultText != null)
            {
                return;
            }

            defaultText = new StaticText
            {
                Format = dynamicText.Format,
                ModificationTime = DateTime.UtcNow,
                ModificationUser = dynamicText.ModificationUser,
                Name = dynamicText.Name,
                Culture = defaultCulture,
                DictionaryScope = dictionaryScope,
            };

            switch (actionForDefaultCulture)
            {
                case IfDefaultNotExistAction.DoNothing:
                    return;
                case IfDefaultNotExistAction.CreateEmpty:
                    defaultText.Text = string.Empty;
                    dao.Create(defaultText);
                    break;
                case IfDefaultNotExistAction.CreateTextCopy:
                    defaultText.Text = dynamicText.Text;
                    dao.Create(defaultText);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionForDefaultCulture), actionForDefaultCulture,
                        "Invalid default language save action");
            }
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var dao = new StaticTextDao(dbContext.StaticText);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);
                var staticTextList = dao.FindByNameAndScope(name, dictionaryScope, dbContext.CultureHierarchy);

                if (staticTextList.Count == 0)
                {
                    return;
                }

                foreach (var staticText in staticTextList)
                {
                    dao.Delete(staticText);
                }

                dbContext.SaveChanges();
            }
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);
                var dao = new StaticTextDao(dbContext.StaticText);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);
                var staticText = dao.FindByNameAndCultureAndScope(
                    name, culture, dictionaryScope, dbContext.CultureHierarchy
                );

                if (staticText == null)
                {
                    return;
                }

                dao.Delete(staticText);

                dbContext.SaveChanges();
            }
        }

        private Culture CreateCulture(IDatabaseStaticTextContext dbContext, string cultureName)
        {
            var cultureDao = new CultureDao(dbContext.Culture);
            return cultureDao.Create(new Culture() {Id = 0, Name = cultureName});
        }

        private void CreateCultureHierarchy(IDatabaseStaticTextContext dbContext, Culture culture)
        {
            var cultureHierarchyDao = new CultureHierarchyDao(dbContext.CultureHierarchy);
            var cultureDao = new CultureDao(dbContext.Culture);

            cultureHierarchyDao.MakeCultureSelfReferencing(culture);

            var defaultCultureName = m_configuration.DefaultCulture.Name;
            var defaultCulture = cultureDao.FindByName(defaultCultureName);

            var cultureInfo = new CultureInfo(culture.Name);
            if (cultureInfo.IsNeutralCulture) //Just reference to default culture
            {
                cultureHierarchyDao.MakeCultureReference(culture, defaultCulture, 1);
            }
            else
            {
                var parentCultureName = cultureInfo.Parent.Name;
                var parentCulture = cultureDao.FindByName(parentCultureName);
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
            var dsDao = new DictionaryScopeDao(dbContext.DictionaryScope);
            return dsDao.Create(new DictionaryScope() {Name = dictionaryScopeName});
        }
    }
}
