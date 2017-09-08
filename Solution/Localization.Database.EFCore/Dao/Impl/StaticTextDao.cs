using System.Linq;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class StaticTextDao : GenericDao<StaticText, int>
    {
        public StaticTextDao(DbSet<StaticText> dbSet) : base(dbSet)
        {
        }

        public IStaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)
        {
            StaticText result = DbSet
                .Where(w => w.Name == name && w.Culture == culture && w.DictionaryScope == dictionaryScope)
                .DefaultIfEmpty()
                .First();

            return result;
        }

        public IStaticText[] FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)
        {
            StaticText[] result = DbSet
                .Where(w => w.Culture == culture && w.DictionaryScope == dictionaryScope)
                .DefaultIfEmpty()
                .ToArray();

            return result;
        }
    }
}