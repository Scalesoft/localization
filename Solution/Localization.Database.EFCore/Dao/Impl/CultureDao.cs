using System.Linq;
using System.Threading.Tasks;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class CultureDao : GenericDao<Culture, int>
    {
        public CultureDao(DbSet<Culture> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        public Culture FindByName(string name)
        {
            return DbSet.FirstOrDefault(c => c.Name == name);
        }

        public async Task<bool> CultureExist(string cultureName)
        {
            return await DbSet.AnyAsync(p => p.Name == cultureName);
        }
    }
}