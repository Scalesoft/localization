using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Database.EFCore.Entity;
using Scalesoft.Localization.Database.EFCore.Logging;

namespace Scalesoft.Localization.Database.EFCore.Dao.Impl
{
    public class CultureDao : GenericDao<Culture, int>
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        public CultureDao(DbSet<Culture> dbSet) : base(dbSet)
        {
            //Should be empty.
        }

        /// <summary>
        /// Finds Culture in dbSet
        /// </summary>
        /// <param name="name">Culture name e.g. en, en-US, cs, cs-CZ</param>
        /// <returns>Instance of the culture in database. If culture is not in database returns null.</returns>
        public Culture FindByName(string name)
        {
            Culture result = null;
            try
            {
                result = DbSet.FirstOrDefault(c => c.Name == name);
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

        /// <summary>
        /// Finds Culture in dbSet
        /// </summary>
        /// <param name="id">Culture database id</param>
        /// <returns>Instance of the culture in database. If Culture is not in database returns null.</returns>
        public Culture FindById(int id)
        {
            Culture result = null;
            try
            {
                result = DbSet.FirstOrDefault(c => c.Id == id);
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

        /// <summary>
        /// Returns true if the specified cultureName exists in dbSet.
        /// </summary>
        /// <param name="cultureName">Culture name to find.</param>
        /// <returns>True if culture exists in dbSet</returns>
        public async Task<bool> CultureExist(string cultureName)
        {
            return await DbSet.AnyAsync(p => p.Name == cultureName);
        }
    }
}
