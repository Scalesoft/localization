using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Database.EFCore.Entity;
using Scalesoft.Localization.Database.EFCore.Logging;

namespace Scalesoft.Localization.Database.EFCore.Dao.Impl
{
    public class DictionaryScopeDao : GenericDao<DictionaryScope, int>
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public DictionaryScopeDao(DbSet<DictionaryScope> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        public DictionaryScope FindByName(string name)
        {
            DictionaryScope result = null;
            try
            {
                result = DbSet.FirstOrDefault(s => s.Name == name);
            }
            catch (ArgumentNullException e)
            {
                if (Logger.IsWarningEnabled())
                {
                    Logger.LogWarning(new EventId(e.GetHashCode()), e, "ArgumentNullException in method FindByName(string name)");
                }
            }

            return result;
        }
    }
}