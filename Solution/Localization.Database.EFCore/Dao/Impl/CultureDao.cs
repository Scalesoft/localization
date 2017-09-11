using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class CultureDao : GenericDao<Culture, int>
    {
        public CultureDao(DbSet<Culture> dbSet) : base(dbSet)
        {
        }

        public Culture FindByName(string name)
        {
            return DbSet.FirstOrDefault(c => c.Name == name);
        }

        public bool CultureExist(Culture culture)
        {
            return DbSet.Any(p => p.Name == culture.Name);
        }
    }
}