using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Util;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Dao.Impl;
using Localization.Database.EFCore.Data;
using Microsoft.Extensions.Localization;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.EntityBuilder;
using Localization.Database.EFCore.Logging;

namespace Localization.Database.EFCore.Service
{
    public sealed class DatabaseTranslateService : DatabaseServiceBase, IDatabaseTranslateService
    {
        public DatabaseTranslateService(Func<IDatabaseStaticTextContext> dbContext, IConfiguration configuration)
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
            //Should be empty.
        }

        /// <summary>
        /// Checks if cultures from configuration file are in database.
        /// </summary>
        public void CheckCulturesInDatabase()
        {
            CheckCulturesInDatabaseAsync().GetAwaiter().GetResult();
        }

        private async Task CheckCulturesInDatabaseAsync()
        {
            var supportedCultures = m_configuration.SupportedCultures();
            foreach (var supportedCulture in supportedCultures)
            {
                await CreateCultureInDatabase(supportedCulture);
                await CreateCultureHierarchySelfReference(supportedCulture);
            }

            await CreateCultureHierarchy(supportedCultures, m_configuration.DefaultCulture());
        }

        /// <summary>
        /// Creates culture in database (if not already exists).
        /// </summary>
        /// <param name="cultureInfo">Culture to create</param>
        /// <returns>False if culture is already in database.</returns>
        private async Task<bool> CreateCultureInDatabase(CultureInfo cultureInfo)
        {
            var cultureName = cultureInfo.Name;

            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var cultureDao = new CultureDao(dbContext.Culture);

                var cultureExist = await cultureDao.CultureExist(cultureName);
                if (!cultureExist)
                {
                    var culture = new CultureBuilder().Name(cultureName).Build();

                    cultureDao.Create(culture);
                    dbContext.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates for given culture self reference in culture hierarchy tree with level 0 (if not already exists).
        /// </summary>
        /// <param name="cultureInfo">Culture</param>
        private async Task CreateCultureHierarchySelfReference(CultureInfo cultureInfo)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var cultureDao = new CultureDao(dbContext.Culture);
                var culture = cultureDao.FindByName(cultureInfo.Name);

                var cultureHierarchyDao = new CultureHierarchyDao(dbContext.CultureHierarchy);

                var isCultureSelfReferencing = await cultureHierarchyDao.IsCultureSelfReferencing(culture);
                if (!isCultureSelfReferencing)
                {
                    cultureHierarchyDao.MakeCultureSelfReferencing(culture);
                    dbContext.SaveChanges();
                }
            }
        }

        private async Task CreateCultureHierarchy(IImmutableList<CultureInfo> supportedCultures, CultureInfo defaultCulture)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var cultureHierarchyDao = new CultureHierarchyDao(dbContext.CultureHierarchy);
                var cultureDao = new CultureDao(dbContext.Culture);

                var defaultCultureEntity = cultureDao.FindByName(defaultCulture.Name);

                IList<CultureInfo> supportedCulturesWithoutDefault = supportedCultures.ToList();
                supportedCulturesWithoutDefault.Remove(defaultCulture);

                foreach (var supportedCultureA in supportedCulturesWithoutDefault)
                {
                    var cultureEntityA = cultureDao.FindByName(supportedCultureA.Name);

                    foreach (var supportedCultureB in supportedCultures)
                    {
                        if (supportedCultureA.Name == supportedCultureB.Name)
                        {
                            continue;
                        }

                        var cultureEntityB = cultureDao.FindByName(supportedCultureB.Name);

                        if (supportedCultureB.Parent.Equals(supportedCultureA))
                        {
                            var isCultureBReferencingA =
                                await cultureHierarchyDao.IsCultureReferencing(cultureEntityB, cultureEntityA);
                            if (!isCultureBReferencingA)
                            {
                                cultureHierarchyDao.MakeCultureReference(cultureEntityB, cultureEntityA, 1);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }

                foreach (var supportedCulture in supportedCulturesWithoutDefault)
                {
                    if (supportedCulture.IsNeutralCulture)
                    {
                        var cultureEntity = cultureDao.FindByName(supportedCulture.Name);

                        var isCultureReferencingDefaultCulture =
                            await cultureHierarchyDao.IsCultureReferencing(cultureEntity, defaultCultureEntity);
                        if (!isCultureReferencingDefaultCulture)
                        {
                            cultureHierarchyDao.MakeCultureReference(cultureEntity, defaultCultureEntity, 1);
                            dbContext.SaveChanges();
                        }
                    }
                }

                //
                foreach (var supportedCultureA in supportedCultures)
                {
                    var cultureEntityA = cultureDao.FindByName(supportedCultureA.Name);

                    foreach (var supportedCultureB in supportedCultures)
                    {
                        if (supportedCultureA.Name == supportedCultureB.Name || supportedCultureB.Name == defaultCultureEntity.Name)
                        {
                            continue;
                        }

                        var cultureEntityB = cultureDao.FindByName(supportedCultureB.Name);
                        if (supportedCultureB.Parent.Equals(supportedCultureA))
                        {
                            if (cultureEntityA.Name != defaultCultureEntity.Name)
                            {

                                var isCultureAReferencingDefaultCulture =
                                    await cultureHierarchyDao.IsCultureReferencing(cultureEntityA, defaultCultureEntity);
                                if (isCultureAReferencingDefaultCulture)
                                {
                                    var isCultureBReferencingDefaultCulture =
                                        await cultureHierarchyDao.IsCultureReferencing(cultureEntityB, defaultCultureEntity);
                                    if (!isCultureBReferencingDefaultCulture)
                                    {
                                        cultureHierarchyDao.MakeCultureReference(cultureEntityB, defaultCultureEntity, 2);
                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public LocalizedString DatabaseTranslate(string text, CultureInfo cultureInfo, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var culture = GetCultureByNameOrGetDefault(dbContext, cultureInfo.Name);
                var dictionaryScope = GetDictionaryScope(dbContext, scope);

                var staticTextDao = new StaticTextDao(dbContext.StaticText);
                IStaticText dbResult = staticTextDao.FindByNameAndCultureAndScope(text, culture, dictionaryScope, dbContext.CultureHierarchy);

                if (dbResult == null)
                {
                    return null;
                }
                return new LocalizedString(text, dbResult.Text, false);
            }
        }

        public LocalizedString DatabaseTranslateFormat(string text, object[] parameters, CultureInfo cultureInfo, string scope)
        {
            using (var dbContext = m_dbContextFunc.Invoke())
            {
                var cultureDao = new CultureDao(dbContext.Culture);
                var culture = cultureDao.FindByName(cultureInfo.Name);

                var dictionaryScopeDao = new DictionaryScopeDao(dbContext.DictionaryScope);
                var dictionaryScope = dictionaryScopeDao.FindByName(scope);

                var staticTextDao = new StaticTextDao(dbContext.StaticText);
                IStaticText dbResult = staticTextDao.FindByNameAndCultureAndScope(text, culture, dictionaryScope, dbContext.CultureHierarchy);

                return new LocalizedString(text, dbResult.Text, false);
            }
        }

        public LocalizedString DatabaseTranslatePluralization(string text, int number, CultureInfo cultureInfo, string scope)
        {
            //TODO translate pluralized text using database
            throw new NotImplementedException();
        }

        public LocalizedString DatabaseTranslateConstant(string text, CultureInfo cultureInfo, string scope)
        {
            //TODO translate constants using database
            throw new NotImplementedException();
        }


    }
}
