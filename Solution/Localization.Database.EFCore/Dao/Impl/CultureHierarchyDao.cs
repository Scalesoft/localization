using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class CultureHierarchyDao : GenericDao<CultureHierarchy, int>
    {
        public CultureHierarchyDao(DbSet<CultureHierarchy> dbSet) : base(dbSet)
        {
        }

        public IQueryable<Culture> FindParentCultures(Culture culture)
        {
            return DbSet.Where(p => p.Culture.Id == culture.Id).OrderBy(p => p.LevelProperty).Select(p => p.ParentCulture);
        }

        public bool IsCultureSelfReferencing(Culture culture)
        {
            return DbSet.Any(p => p.LevelProperty == 0 && p.Culture.Name == culture.Name && p.ParentCulture.Name == culture.Name);
        }

        public bool IsCultureReferencing(Culture culture, Culture parentCulture)
        {
            return DbSet.Any(p => p.Culture.Name == culture.Name && p.ParentCulture.Name == parentCulture.Name);
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