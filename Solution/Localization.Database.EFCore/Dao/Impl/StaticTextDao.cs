using System;
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
                IQueryable<CultureHierarchy> hierarchies = cultureHierarchies
                    .Select(t => t);

                IQueryable<StaticText> result = DbSet
                    .Where(w => w.Name == name && w.DictionaryScope == dictionaryScope)
                    .Join(
                        hierarchies
                            .Where(hierarchyCulture => hierarchyCulture.Culture.Id == culture.Id)
                        , text => text.Culture.Id, hierarchy => hierarchy.ParentCulture.Id
                        , (text, hierarchy) => new {text, hierarchy})
                    .OrderBy(r => r.hierarchy.LevelProperty)
                    .Take(4)
                    .Select(r => r.text);

                resultValue = result.DefaultIfEmpty().First();
            }            
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e, "ArgumentNullException in method FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope, DbSet < CultureHierarchy > cultureHierarchies)");
                }
            }

            if (resultValue != null)
            {
                resultValue.DictionaryScope = dictionaryScope;
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
                    Logger.LogWarning(new EventId(e.GetHashCode()), e, "ArgumentNullException in method FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)");
                }
            }

            return result;
        }
    }
}