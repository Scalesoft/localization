using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        //Checks if supported cultures are in database as well
        public void CheckCultures()
        {
            IList<CultureInfo> supportedCultures = Configuration.SupportedCultures();

            Culture[] culturesToCheck = new Culture[supportedCultures.Count + 1];
            culturesToCheck[0] = new CultureBuilder().Name(Configuration.DefaultCulture().Name).Build();

            for (int i = 0; i < supportedCultures.Count; i++)
            {
                culturesToCheck[i+1] = new CultureBuilder().Name(supportedCultures[i].Name).Build();
            }

            CultureDao cultureDao = new CultureDao(DbContext.Culture);
            foreach (Culture cultureToCheck in culturesToCheck)
            {

                if (!cultureDao.CultureExist(cultureToCheck))
                {
                    cultureDao.Create(cultureToCheck);
                    ((DbContext)DbContext).SaveChanges();
                }
                CheckCultureHierarchySelfReference(cultureDao.FindByName(cultureToCheck.Name));
                CheckCultureHierarchy(cultureDao.FindByName(cultureToCheck.Name), Configuration.SupportedCultures().ToArray());
                
            }
        }

        private void CheckCultureHierarchySelfReference(Culture culture)
        {
            CultureHierarchyDao cultureHierarchyDao = new CultureHierarchyDao(DbContext.CultureHierarchy);
            if (!cultureHierarchyDao.IsCultureSelfReferencing(culture))
            {
                cultureHierarchyDao.MakeCultureSelfReferencing(culture);
                ((DbContext)DbContext).SaveChanges();
            }
        }

        private void CheckCultureHierarchy(Culture culture, CultureInfo[] culturesToCheck)
        {
            CultureHierarchyDao cultureHierarchyDao = new CultureHierarchyDao(DbContext.CultureHierarchy);
            CultureDao cultureDao = new CultureDao(DbContext.Culture);

            foreach (CultureInfo cultureToCheck in culturesToCheck)
            {
                CultureInfo cultureInfo = new CultureInfo(culture.Name);
                if (cultureInfo.IsNeutralCulture)
                {
                    Culture parentCulture = cultureDao.FindByName(Configuration.DefaultCulture().Name);
                    if (!cultureHierarchyDao.IsCultureReferencing(culture, parentCulture))
                    {
                        cultureHierarchyDao.MakeCultureReference(culture, parentCulture, 2);
                        ((DbContext)DbContext).SaveChanges();
                    }
                }

                if (cultureInfo.Parent.Equals(cultureToCheck))
                {
                    Culture parentCulture = cultureDao.FindByName(cultureToCheck.Name);

                    if (!cultureHierarchyDao.IsCultureReferencing(culture, parentCulture))
                    {
                        cultureHierarchyDao.MakeCultureReference(culture, parentCulture, 1);
                        ((DbContext)DbContext).SaveChanges();
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