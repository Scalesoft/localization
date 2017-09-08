using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class CultureHierarchyDao : GenericDao<CultureHierarchy, int>
    {
        protected CultureHierarchyDao(DbSet<CultureHierarchy> dbSet) : base(dbSet)
        {
        }

        public IQueryable<Culture> FindParentCultures(Culture culture)
        {
            return DbSet.Where(p => p.CultureId == culture.Id).OrderBy(p => p.LevelProperty).Select(p => p.ParentCulture);
        }
    }
}