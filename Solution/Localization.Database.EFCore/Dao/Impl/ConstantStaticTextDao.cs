using System.Linq;
using Localization.Database.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class ConstantStaticTextDao : GenericDao<ConstantStaticText, int>
    {
        protected ConstantStaticTextDao(DbSet<ConstantStaticText> dbSet) : base(dbSet)
        {
        }

        public ConstantStaticText FindByName(string name)
        {
            return m_dbSet.First(w => w.Name == name);
        }

        public ConstantStaticText FindByNameAndCulture(string name, Culture culture)
        {
            return m_dbSet.First(w => w.Name == name && w.Culture == culture);
        }

        public ConstantStaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)
        {
            return m_dbSet.First(w => w.Name == name && w.Culture == culture && w.DictionaryScope == dictionaryScope);
        }

    }
}