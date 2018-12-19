using System;
using System.Collections.Generic;
using System.Linq;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class StaticTextDao : GenericDao<StaticText, int>
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public StaticTextDao(DbSet<StaticText> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        public StaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope,
            DbSet<CultureHierarchy> cultureHierarchies)
        {
            StaticText resultValue = null;
            try
            {
                var hierarchies = cultureHierarchies
                    .Select(t => t)
                    .Where(hierarchyCulture => hierarchyCulture.Culture.Id == culture.Id);

                var result = DbSet
                    .Where(w => w.Name == name && w.DictionaryScope == dictionaryScope)
                    .Include(x => x.Culture)
                    .Join(hierarchies,
                        text => text.Culture.Id,
                        hierarchy => hierarchy.ParentCulture.Id,
                        (text, hierarchy) => new {text, hierarchy})
                    .OrderBy(r => r.hierarchy.LevelProperty)
                    .Take(4)
                    .Select(r => r.text);

                resultValue = result.DefaultIfEmpty().First();
            }
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(
                        new EventId(e.GetHashCode()), e,
                        "ArgumentNullException in method FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope, DbSet < CultureHierarchy > cultureHierarchies)"
                    );
                }
            }

            return resultValue;
        }

        public IList<StaticText> FindByNameAndScope(string name, DictionaryScope dictionaryScope,
            DbSet<CultureHierarchy> cultureHierarchies)
        {
            List<StaticText> resultValue = null;
            try
            {
                var hierarchies = cultureHierarchies
                    .Select(t => t);

                var result = DbSet
                    .Where(w => w.Name == name && w.DictionaryScope == dictionaryScope);
                //.Include(x => x.Culture)
                //.Join(hierarchies,
                //    text => text.Culture.Id,
                //    hierarchy => hierarchy.ParentCulture.Id,
                //    (text, hierarchy) => new {text, hierarchy})
                //.OrderBy(r => r.hierarchy.LevelProperty)
                //.Take(4) Why is this restriction used in FindByNameAndCultureAndScope method?
                //.Select(r => r.text);

                resultValue = result.Distinct().ToList();
            }
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e,
                        "ArgumentNullException in method FindByNameAndScope");
                }
            }

            return resultValue;
        }

        public StaticText[] FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)
        {
            StaticText[] result = null;
            try
            {
                result = DbSet
                    .Where(w => w.Culture == culture && w.DictionaryScope == dictionaryScope)
                    .ToArray();
            }
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e,
                        "ArgumentNullException in method FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)");
                }
            }

            return result;
        }
    }
}
