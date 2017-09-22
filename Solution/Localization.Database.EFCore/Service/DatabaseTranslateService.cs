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
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Service
{
    public sealed class DatabaseTranslateService : DatabaseServiceBase, IDatabaseTranslateService
    {     
        public DatabaseTranslateService(IDatabaseStaticTextContext dbContext, IConfiguration configuration) 
            : base(LogProvider.GetCurrentClassLogger(), dbContext, configuration)
        {
            //Should be empty.
        }

        /// <summary>
        /// Checks if cultures from configuration file are in database.
        /// </summary>
        public async void CheckCulturesInDatabase()
        {
            IImmutableList<CultureInfo> supportedCultures = Configuration.SupportedCultures();
            foreach (CultureInfo supportedCulture in supportedCultures)
            {
                await CreateCultureInDatabase(supportedCulture);
                await CreateCultureHierarchySelfReference(supportedCulture);
            }
            
            await CreateCultureHierarchy(supportedCultures, Configuration.DefaultCulture());
        }

        /// <summary>
        /// Creates culture in database (if not already exists).
        /// </summary>
        /// <param name="cultureInfo">Culture to create</param>
        /// <returns>False if culture is already in database.</returns>
        private async Task<bool> CreateCultureInDatabase(CultureInfo cultureInfo)
        {
            string cultureName = cultureInfo.Name;

            CultureDao cultureDao = new CultureDao(DbContext.Culture);

            bool cultureExist = await cultureDao.CultureExist(cultureName);
            if (!cultureExist)
            {
                Culture culture = new CultureBuilder().Name(cultureName).Build();

                cultureDao.Create(culture);
                ((DbContext)DbContext).SaveChanges();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates for given culture self reference in culture hierarchy tree with level 0 (if not already exists).
        /// </summary>
        /// <param name="cultureInfo">Culture</param>
        private async Task CreateCultureHierarchySelfReference(CultureInfo cultureInfo)
        {
            CultureDao cultureDao = new CultureDao(DbContext.Culture);
            Culture culture = cultureDao.FindByName(cultureInfo.Name);

            CultureHierarchyDao cultureHierarchyDao = new CultureHierarchyDao(DbContext.CultureHierarchy);

            bool isCultureSelfReferencing = await cultureHierarchyDao.IsCultureSelfReferencing(culture);
            if (!isCultureSelfReferencing)
            {
                cultureHierarchyDao.MakeCultureSelfReferencing(culture);
                ((DbContext)DbContext).SaveChanges();
            }
        }

        private async Task CreateCultureHierarchy(IImmutableList<CultureInfo> supportedCultures, CultureInfo defaultCulture)
        {
            CultureHierarchyDao cultureHierarchyDao = new CultureHierarchyDao(DbContext.CultureHierarchy);
            CultureDao cultureDao = new CultureDao(DbContext.Culture);

            Culture defaultCultureEntity = cultureDao.FindByName(defaultCulture.Name);

            IList<CultureInfo> supportedCulturesWithoutDefault = supportedCultures.ToList();
            supportedCulturesWithoutDefault.Remove(defaultCulture);

            foreach (CultureInfo supportedCultureA in supportedCulturesWithoutDefault)
            {
                Culture cultureEntityA = cultureDao.FindByName(supportedCultureA.Name);

                foreach (CultureInfo supportedCultureB in supportedCultures)
                {
                    if (supportedCultureA.Name == supportedCultureB.Name)
                    {
                        continue;
                    }

                    Culture cultureEntityB = cultureDao.FindByName(supportedCultureB.Name);

                    if (supportedCultureB.Parent.Equals(supportedCultureA))
                    {
                        bool isCultureBReferencingA =
                            await cultureHierarchyDao.IsCultureReferencing(cultureEntityB, cultureEntityA);
                        if (!isCultureBReferencingA)
                        {
                            cultureHierarchyDao.MakeCultureReference(cultureEntityB, cultureEntityA, 1);
                            ((DbContext)DbContext).SaveChanges();
                        }
                    }
                }
            }

            foreach (CultureInfo supportedCulture in supportedCulturesWithoutDefault)
            {
                if (supportedCulture.IsNeutralCulture)
                {
                    Culture cultureEntity = cultureDao.FindByName(supportedCulture.Name);

                    bool isCultureReferencingDefaultCulture =
                        await cultureHierarchyDao.IsCultureReferencing(cultureEntity, defaultCultureEntity);
                    if (!isCultureReferencingDefaultCulture)
                    {
                        cultureHierarchyDao.MakeCultureReference(cultureEntity, defaultCultureEntity, 1);
                        ((DbContext)DbContext).SaveChanges();
                    }
                }
            }

            //
            foreach (CultureInfo supportedCultureA in supportedCultures)
            {
                Culture cultureEntityA = cultureDao.FindByName(supportedCultureA.Name);

                foreach (CultureInfo supportedCultureB in supportedCultures)
                {
                    if (supportedCultureA.Name == supportedCultureB.Name || supportedCultureB.Name == defaultCultureEntity.Name)
                    {
                        continue;
                    }

                    Culture cultureEntityB = cultureDao.FindByName(supportedCultureB.Name);
                    if (supportedCultureB.Parent.Equals(supportedCultureA))
                    {
                        if (cultureEntityA.Name != defaultCultureEntity.Name)
                        {

                            bool isCultureAReferencingDefaultCulture =
                                await cultureHierarchyDao.IsCultureReferencing(cultureEntityA, defaultCultureEntity);
                            if(isCultureAReferencingDefaultCulture)
                            {
                                bool isCultureBReferencingDefaultCulture =
                                    await cultureHierarchyDao.IsCultureReferencing(cultureEntityB, defaultCultureEntity);
                                if (!isCultureBReferencingDefaultCulture)
                                {
                                    cultureHierarchyDao.MakeCultureReference(cultureEntityB, defaultCultureEntity, 2);
                                    ((DbContext)DbContext).SaveChanges();
                                }                               
                            }                        
                        }
                    }
                }
            }
        }


        public LocalizedString DatabaseTranslate(string text, CultureInfo cultureInfo, string scope)
        {
            Culture culture = GetCulture(cultureInfo.Name);
            DictionaryScope dictionaryScope = GetDictionaryScope(scope);

            StaticTextDao staticTextDao = new StaticTextDao(DbContext.StaticText);
            IStaticText dbResult = staticTextDao.FindByNameAndCultureAndScope(text, culture, dictionaryScope, DbContext.CultureHierarchy);

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
            IStaticText dbResult = staticTextDao.FindByNameAndCultureAndScope(text, culture, dictionaryScope, DbContext.CultureHierarchy);

            return new LocalizedString(text, dbResult.Text, false);
        }

        public LocalizedString DatabaseTranslatePluralization(string text, int number, CultureInfo cultureInfo, string scope)
        {
            //TODO!!!
            throw new NotImplementedException();
        }

        public LocalizedString DatabaseTranslateConstant(string text, CultureInfo cultureInfo, string scope)
        {
            //TODO!!!
            throw new NotImplementedException();
        }

        
    }
}