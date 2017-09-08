using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class DictionaryScopeDao : GenericDao<DictionaryScope, int>
    {
        public DictionaryScopeDao(DbSet<DictionaryScope> dbSet) : base(dbSet)
        {
        }

        public DictionaryScope FindByName(string name)
        {
            return DbSet.FirstOrDefault(s => s.Name == name);
        }
    }
}