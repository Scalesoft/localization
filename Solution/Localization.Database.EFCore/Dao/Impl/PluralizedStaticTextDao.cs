using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class PluralizedStaticTextDao : GenericDao<PluralizedStaticText, int>
    {
        public PluralizedStaticTextDao(DbSet<PluralizedStaticText> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        public PluralizedStaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)
        {
            return DbSet.First(w => w.Name == name && w.Culture == culture && w.DictionaryScope == dictionaryScope);
        }

        public PluralizedStaticText[] FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)
        {
            PluralizedStaticText[] result = DbSet
                .Where(w => w.Culture == culture && w.DictionaryScope == dictionaryScope)
                .DefaultIfEmpty()
                .ToArray();

            return result;
        }
    }
}