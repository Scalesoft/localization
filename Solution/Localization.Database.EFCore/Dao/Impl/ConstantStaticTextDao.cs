using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class ConstantStaticTextDao : GenericDao<ConstantStaticText, int>
    {
        public ConstantStaticTextDao(DbSet<ConstantStaticText> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        public ConstantStaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)
        {
            return DbSet.First(w => w.Name == name && w.Culture == culture && w.DictionaryScope == dictionaryScope);
        }

        public ConstantStaticText[] FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)
        {
            ConstantStaticText[] result = DbSet
                .Where(w => w.Culture == culture && w.DictionaryScope == dictionaryScope)
                .DefaultIfEmpty()
                .ToArray();

            return result;
        }
    }
}