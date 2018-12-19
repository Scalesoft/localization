using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Service
{
    public class DatabaseTranslateService : DatabaseServiceBase, IDatabaseTranslateService
    {
        public DatabaseTranslateService(
            ILogger logger, CultureUoW cultureUoW, IConfiguration configuration
        ) : base(logger, cultureUoW, configuration)
        {
        }

        public LocalizedString DatabaseTranslate(string text, CultureInfo cultureInfo, string scope)
        {
            throw new System.NotImplementedException();
        }

        public LocalizedString DatabaseTranslateFormat(string text, object[] parameters, CultureInfo cultureInfo,
            string scope)
        {
            throw new System.NotImplementedException();
        }

        public LocalizedString DatabaseTranslatePluralization(string text, int number, CultureInfo cultureInfo,
            string scope)
        {
            throw new System.NotImplementedException();
        }

        public LocalizedString DatabaseTranslateConstant(string text, CultureInfo cultureInfo, string scope)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Checks if cultures from configuration file are in database.
        /// </summary>
        public void CheckCulturesInDatabase()
        {
            var supportedCultures = m_configuration.SupportedCultures();
            var availableCultures = m_cultureUoW.FindAllCultures();

            foreach (var supportedCulture in supportedCultures)
            {
                if (availableCultures.All(x => x.Name != supportedCulture.Name))
                {
                    var id = m_cultureUoW.AddCulture(supportedCulture.Name);
                    var culture = m_cultureUoW.GetCultureById(id);

                    availableCultures.Add(culture);

                    //TODO create hierarchy
                }
            }
        }
    }
}
