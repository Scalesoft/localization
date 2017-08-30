using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class DictionaryScopeDao : GenericDao<DictionaryScope, int>
    {
        protected DictionaryScopeDao(DbSet<DictionaryScope> dbSet) : base(dbSet)
        {
        }
    }
}