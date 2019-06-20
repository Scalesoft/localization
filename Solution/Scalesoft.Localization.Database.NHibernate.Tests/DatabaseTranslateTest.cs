using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.IoC;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Core.Util;
using Scalesoft.Localization.Database.NHibernate.Tests.Helper;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Scalesoft.Localization.Database.NHibernate.Tests
{
    [TestClass]
    public class DatabaseTranslateTest
    {
        private DatabaseLocalizationManager m_databaseLocalizationManager;
        private ISessionFactory m_sessionFactory;
        private StaticTextUoW m_staticTextUoW;
        private LocalizationConfiguration m_localizationConfiguration;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionFactory = NHibernateConfigurator.GetSessionFactory(nameof(DatabaseTranslateTest));

            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("cs"),
                    new CultureInfo("es"),
                    new CultureInfo("jp"),
                    new CultureInfo("ru"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key
            };

            m_localizationConfiguration = localizationConfiguration;

            var services = new ServiceCollection();
            services.AddLocalizationCore(localizationConfiguration, new NHibernateDatabaseConfiguration(m_sessionFactory));
            services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            var container = services.BuildServiceProvider();
            var dbTranslateService = container.GetRequiredService<IDatabaseTranslateService>();
            var dbDynamicTextService = container.GetRequiredService<IDatabaseDynamicTextService>();
            m_databaseLocalizationManager =
                new DatabaseLocalizationManager(localizationConfiguration, dbTranslateService, dbDynamicTextService);

            AddTestData(container);
        }

        private void AddTestData(ServiceProvider container)
        {
            var cultureUoW = container.GetRequiredService<CultureUoW>();
            cultureUoW.AddCulture("cs");
            cultureUoW.AddCulture("en");
            cultureUoW.AddCulture("es");
            cultureUoW.AddCulture("jp");
            cultureUoW.AddCulture("ru");

            var scopeUoW = container.GetRequiredService<DictionaryScopeUoW>();
            scopeUoW.AddScope("home");
            scopeUoW.AddScope("global");

            var now = DateTime.UtcNow;
            var staticTextUoW = container.GetRequiredService<StaticTextUoW>();
            m_staticTextUoW = staticTextUoW;

            staticTextUoW.AddStaticText("support", 0, "Podpora", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("about", 0, "O portálu", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("copyright", 0, "Copyright", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("contacts", 0, "Kontakty", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("links", 0, "Odkazy", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("howtocite", 0, "Jak citovat", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("feedback", 0, "Připomínky", "cs", "home", "user", now);
            staticTextUoW.AddStaticText("cancel", 0, "Zrušit", "cs", "home", "user", now);
        }

        [TestMethod]
        public void SimpleTranslateTest()
        {
            var text = m_databaseLocalizationManager.Translate(null, "home", "support");
            Assert.IsNotNull(text);
            Assert.IsNotNull(text.Value);
            Assert.AreNotEqual(0, text.Value.Length);
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
                m_databaseLocalizationManager.Translate(null, scope, key);
            });
        }

        [TestMethod]
        public void TranslateFallbackTest()
        {
            const string scope = "home";
            const string key = "name";
            var defaultCulture = m_localizationConfiguration.DefaultCulture;
            const string defaultValue = "Jméno";

            var now = DateTime.UtcNow;
            
            var nonDefaultCulturesValuePairs = new Dictionary<string, string>
            {
                {"en", "Name"},
                {"es", "Nombre"},
                {"jp", "名前"},
                {"ru", "Имя"},
            };

            var defaultCulturesValuePair = new KeyValuePair<string, string>(defaultCulture.Name, defaultValue);
            
            foreach (var culturesValuePair in nonDefaultCulturesValuePairs)
            {
                m_staticTextUoW.AddStaticText("name", 0, culturesValuePair.Value, culturesValuePair.Key, "home", "user", now);
            }

            m_staticTextUoW.AddStaticText("name", 0, defaultCulturesValuePair.Value, defaultCulturesValuePair.Key, "home", "user", now);
            
            foreach (var culturesValuePair in nonDefaultCulturesValuePairs)
            {
                var translateResult = m_databaseLocalizationManager.Translate(new CultureInfo(culturesValuePair.Key), scope, key);
                Assert.AreEqual(culturesValuePair.Value, translateResult.Value);
            }

            var defaultCultureTranslateResult = m_databaseLocalizationManager.Translate(new CultureInfo(defaultCulturesValuePair.Key), scope, key);
            Assert.AreEqual(defaultCulturesValuePair.Value, defaultCultureTranslateResult.Value);

            foreach (var culturesValuePair in nonDefaultCulturesValuePairs)
            {
                m_staticTextUoW.Delete(key, culturesValuePair.Key, scope);
            }

            foreach (var culturesValuePair in nonDefaultCulturesValuePairs)
            {
                var translateResult = m_databaseLocalizationManager.Translate(new CultureInfo(culturesValuePair.Key), scope, key);
                Assert.AreEqual(defaultCultureTranslateResult.Value, translateResult.Value);
            }
        }

        [TestMethod]
        public void GetDynamicTextWithoutFallbackTest()
        {
            const string key = "key123";
            const string scope = "home";
            const string csText = "Czech label";
            const string enText = "English label";
            
            var now = DateTime.UtcNow;
            m_staticTextUoW.AddStaticText(key, 0, csText, "cs", scope, "user", now);
            m_staticTextUoW.AddStaticText(key, 0, enText, "en", scope, "user", now);

            var czech = m_databaseLocalizationManager.GetDynamicText(key, scope, new CultureInfo("cs"));
            var english = m_databaseLocalizationManager.GetDynamicText(key, scope, new CultureInfo("en"));
            var spanish = m_databaseLocalizationManager.GetDynamicText(key, scope, new CultureInfo("es"));

            Assert.AreEqual(csText, czech.Text);
            Assert.AreEqual(enText, english.Text);
            Assert.IsNull(spanish);
        }
    }
}