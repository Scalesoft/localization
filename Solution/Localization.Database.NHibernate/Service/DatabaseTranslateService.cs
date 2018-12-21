using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Resolver;
using Localization.CoreLibrary.Util;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.Database.NHibernate.Service
{
    public class DatabaseTranslateService : DatabaseServiceBase, IDatabaseTranslateService
    {
        private readonly FallbackCultureResolver m_fallbackCultureResolver;
        private readonly CultureHierarchyUoW m_cultureHierarchyUoW;

        public DatabaseTranslateService(
            FallbackCultureResolver fallbackCultureResolver,
            CultureHierarchyUoW cultureHierarchyUoW,
            ILocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            ILogger logger
        ) : base(configuration, cultureUoW, logger)
        {
            m_fallbackCultureResolver = fallbackCultureResolver;
            m_cultureHierarchyUoW = cultureHierarchyUoW;
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
            var supportedCultures = m_configuration.SupportedCultures;
            var availableCultures = m_cultureUoW.FindAllCultures();

            foreach (var supportedCulture in supportedCultures)
            {
                if (availableCultures.Any(x => x.Name == supportedCulture.Name))
                {
                    continue;
                }

                var id = m_cultureUoW.AddCulture(supportedCulture.Name);
                var culture = m_cultureUoW.GetCultureById(id);

                availableCultures.Add(culture);
            }

            var cultureHierarchies = m_cultureHierarchyUoW.FindAllCultureHierarchies();

            foreach (var availableCulture in availableCultures)
            {
                var parentCulture = new CultureInfo(availableCulture.Name);

                byte level = 0;
                while (parentCulture != null)
                {
                    var parentCultureEntity = availableCultures.First(x => x.Name == parentCulture.Name);

                    if (!cultureHierarchies.Any(x =>
                        x.Culture.Equals(availableCulture)
                        && x.ParentCulture.Equals(parentCultureEntity)
                    ))
                    {
                        m_cultureHierarchyUoW.AddCultureHierarchy(
                            availableCulture,
                            parentCultureEntity,
                            level
                        );
                    }

                    level++;
                    parentCulture = m_fallbackCultureResolver.FallbackCulture(parentCulture);
                }
            }
        }
    }
}
