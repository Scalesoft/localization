using System;
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
            //Should be empty.
        }

        public StaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope, DbSet<CultureHierarchy> cultureHierarchies)
        {
            IQueryable<StaticText> result = DbSet
                .Where(w => w.Name == name && w.DictionaryScope == dictionaryScope)
                .Join(
                    cultureHierarchies
                    .Where(hierarchyCulture => hierarchyCulture.Culture == culture)
                    , text => text.Culture.Id, hierarchy => hierarchy.ParentCulture.Id
                    , (text, hierarchy) => new {text, hierarchy})

                .OrderBy(r => r.hierarchy.LevelProperty)     
                .Take(4)
                .Select(r => r.text);

            StaticText resultValue = result.DefaultIfEmpty().First();
            resultValue.DictionaryScope = dictionaryScope;
            resultValue.Culture = culture;

            return resultValue;
        }

        public StaticText[] FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)
        {
            StaticText[] result = DbSet
                .Where(w => w.Culture == culture && w.DictionaryScope == dictionaryScope)
                .ToArray();

            return result;
        }
    }
}