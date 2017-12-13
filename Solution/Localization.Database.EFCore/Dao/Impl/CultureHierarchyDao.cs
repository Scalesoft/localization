using System;
using System.Linq;
using System.Threading.Tasks;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class CultureHierarchyDao : GenericDao<CultureHierarchy, int>
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public CultureHierarchyDao(DbSet<CultureHierarchy> dbSet) : base(dbSet)
        {
            //Should be empty
        }

        /// <summary>
        /// Returns parent cultures for specified culture. 
        /// </summary>
        /// <param name="culture">Culture to which we want parents.</param>
        /// <returns>Returns parent cultures for specified culture. If parent culture does not exist returns null.</returns>
        public IQueryable<Culture> FindParentCultures(Culture culture)
        {
            IQueryable<Culture> result = null;
            try
            {
                result = DbSet.Where(p => p.Culture.Id == culture.Id)
                              .OrderBy(p => p.LevelProperty)
                              .Select(p => p.ParentCulture);
            }
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e, "ArgumentNullException in method FindParentCultures(Culture culture)");
                }
            }

            return result;
        }


        public async Task<bool> IsCultureSelfReferencing(Culture culture)
        {
            return await DbSet.AnyAsync(p => p.LevelProperty == 0 && p.Culture.Name == culture.Name && p.ParentCulture.Name == culture.Name);
        }

        public async Task<bool> IsCultureReferencing(Culture culture, Culture parentCulture)
        {
            return await DbSet.AnyAsync(p => p.Culture.Name == culture.Name && p.ParentCulture.Name == parentCulture.Name);
        }

        public bool MakeCultureSelfReferencing(Culture culture)
        {
            CultureHierarchy newCultureHierarchy = Create(new CultureHierarchy()
            {
                Culture = culture,
                ParentCulture = culture,
                LevelProperty = 0,          
            });
            if (newCultureHierarchy == null)
            {
                return false;
            }

            return true;
        }

        public void MakeCultureReference(Culture culture, Culture parentCulture, byte level)
        {
            Create(new CultureHierarchy()
            {
                Culture = culture,
                ParentCulture = parentCulture,
                LevelProperty = level,
            });
        }


    }
}