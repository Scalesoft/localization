using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Resolver;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate.Service
{
    public class DatabaseTranslateService : DatabaseServiceBase, IDatabaseTranslateService
    {
        private readonly FallbackCultureResolver m_fallbackCultureResolver;
        private readonly CultureHierarchyUoW m_cultureHierarchyUoW;
        private readonly StaticTextUoW m_staticTextUoW;

        public DatabaseTranslateService(
            FallbackCultureResolver fallbackCultureResolver,
            CultureHierarchyUoW cultureHierarchyUoW,
            StaticTextUoW staticTextUoW,
            LocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            DictionaryScopeUoW dictionaryScopeUoW,
            ILogger<DatabaseTranslateService> logger,
            IMemoryCache memoryCache
        ) : base(configuration, cultureUoW, dictionaryScopeUoW, logger, memoryCache)
        {
            m_fallbackCultureResolver = fallbackCultureResolver;
            m_cultureHierarchyUoW = cultureHierarchyUoW;
            m_staticTextUoW = staticTextUoW;
        }

        public LocalizedString DatabaseTranslate(string text, CultureInfo cultureInfo, string scope)
        {
            var culture = GetCachedCultureByNameOrGetDefault(cultureInfo.Name);
            var dictionaryScope = GetCachedDictionaryScope(scope);

            var staticText = m_staticTextUoW.GetByNameAndCultureAndScopeWithHierarchy(
                text, culture.Name, dictionaryScope.Name
            );

            if (staticText == null)
            {
                return null;
            }

            return new LocalizedString(text, staticText.Text, false);
        }

        public LocalizedString DatabaseTranslateFormat(
            string text, object[] parameters, CultureInfo cultureInfo, string scope
        )
        {
            //TODO translate formatted text using database
            throw new System.NotImplementedException();
        }

        public LocalizedString DatabaseTranslatePluralization(
            string text, int number, CultureInfo cultureInfo, string scope
        )
        {
            //TODO translate pluralized text using database
            throw new System.NotImplementedException();
        }

        public LocalizedString DatabaseTranslateConstant(string text, CultureInfo cultureInfo, string scope)
        {
            //TODO translate constants using database
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
