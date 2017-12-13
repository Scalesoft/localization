using System;
using System.Linq;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class PluralizedStaticTextDao : GenericDao<PluralizedStaticText, int>
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public PluralizedStaticTextDao(DbSet<PluralizedStaticText> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        public PluralizedStaticText FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)
        {
            PluralizedStaticText result = null;
            try
            {
                result = DbSet.First(
                    w => w.Name == name && w.Culture == culture && w.DictionaryScope == dictionaryScope);
            }
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e, "ArgumentNullException in method FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)");
                }
            }
            catch (InvalidOperationException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e, "InvalidOperationException in method FindByNameAndCultureAndScope(string name, Culture culture, DictionaryScope dictionaryScope)");
                }
            }

            return result;
        }

        public PluralizedStaticText[] FindAllByCultureAndScope(Culture culture, DictionaryScope dictionaryScope)
        {
            PluralizedStaticText[] result = null;
            try
            {
                result = DbSet
                    .Where(w => w.Culture == culture && w.DictionaryScope == dictionaryScope)
                    .DefaultIfEmpty()
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