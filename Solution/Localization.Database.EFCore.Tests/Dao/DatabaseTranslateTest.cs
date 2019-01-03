using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Localization.Database.EFCore.Data;
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
        private DbContextOptions<StaticTextsContext> m_builderOptions;

        [TestInitialize]
        public void InitTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("cs"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key
            };

            m_builderOptions = new DbContextOptionsBuilder<StaticTextsContext>()
                .UseInMemoryDatabase("DatabaseTranslateTest").Options;

            var dbTranslateService = new DatabaseTranslateService(CreateStaticTextContext, localizationConfiguration);
            var dbDynamicTextService =
                new DatabaseDynamicTextService(CreateStaticTextContext, localizationConfiguration);

            m_databaseLocalizationManager =
                new DatabaseLocalizationManager(localizationConfiguration, dbTranslateService, dbDynamicTextService);
        }

        private IDatabaseStaticTextContext CreateStaticTextContext()
        {
            return new StaticTextsContext(m_builderOptions);
        }

        [TestMethod]
        public void TranslateConcurrentlyTest()
        {
            const string scope = "home";
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
