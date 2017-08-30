using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class CultureDao : GenericDao<Culture, int>
    {
        public CultureDao(DbSet<Culture> dbSet) : base(dbSet)
        {
        }
    }
}