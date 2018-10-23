using System.Collections.Generic;
using System.Threading.Tasks;
using Localization.CoreLibrary;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Data.Impl;
using Localization.Database.EFCore.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.Database.EFCore.Tests.Dao
{
    [TestClass]
    public class DatabaseTranslateTest
    {
        private DatabaseLocalizationManager m_databaseLocalizationManager;

        [TestInitialize]
        public void InitTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = @"localization",
                DefaultCulture = @"cs",
                SupportedCultures = new List<string> { "en", "cs" },
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            var localizationConfiguration = new LocalizationConfiguration(configuration);

            var builderOptions = new DbContextOptionsBuilder<StaticTextsContext>()
                .UseSqlServer(Configuration.ConnectionString).Options;

            var staticTextContext = new StaticTextsContext(builderOptions);

            var dbTranslateService = new DatabaseTranslateService(staticTextContext, localizationConfiguration);
            var dbDynamicTextService = new DatabaseDynamicTextService(staticTextContext, localizationConfiguration);

            m_databaseLocalizationManager = new DatabaseLocalizationManager(localizationConfiguration, dbTranslateService, dbDynamicTextService);
        }

        [TestMethod]
        public void TranslateConcurrentlyTest()
        {
            var scope = "home";
            var keysFromScope = new[]
            {
                "support",
                "about",
                "copyright",
                "contacts",
                "links",
                "howtocite",
                "feedback",
            };

            Parallel.For(0, 1000, iteration =>
            {
                var key = keysFromScope[iteration % keysFromScope.Length];
                m_databaseLocalizationManager.Translate(key, null, scope);
            });
        }
    }
}