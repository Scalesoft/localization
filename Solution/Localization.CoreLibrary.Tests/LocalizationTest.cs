using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests
{
    [TestClass]
    public class LocalizationTest
    {
        [TestCleanup]
        public void LibDeInit()
        {
            Localization.LibDeinit();
        }

        [TestMethod]
        public void InitLibTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es", "cs"}
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            Assert.IsNotNull(Localization.Dictionary);
            Assert.IsNotNull(Localization.Translator);
        }

        [TestMethod]
        public void DoubleInitLibTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es", "cs"}
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            var exceptionThrown = false;
            try
            {
                Localization.Init(localizationConfiguration);
            }
            catch (LocalizationLibraryException)
            {
                exceptionThrown = true;
            }

            if (exceptionThrown)
            {
                Assert.IsTrue(exceptionThrown);
            }
        }

        [TestMethod]
        public void AttachLoggerTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es", "cs"}
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddDebug();

            Localization.AttachLogger(loggerFactory);

            var logger = LogProvider.GetCurrentClassLogger();

            logger.LogWarning("warning LOG test");
            logger.LogCritical("critical LOG test");
        }

        [TestMethod]
        public void TranslateBasic()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es", "cs"},
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString(),
                AutoLoadResources = true
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);
            Localization.Init(localizationConfiguration);

            //Localization.Init("localization.json.config");

            Localization.AttachLogger(new NullLoggerFactory());

            var ls = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst");

            Assert.AreEqual("text-2-odst", ls.Name);
            Assert.IsFalse(ls.ResourceNotFound);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", ls.Value);

            var lsQQ = Localization.Translator.Translate(LocTranslationSource.File, "text-QQ-odst");
            Assert.AreEqual("text-QQ-odst", lsQQ);

            var lsEn = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst", new CultureInfo("en"));

            Assert.AreEqual("text-2-odst", lsEn.Name);
            Assert.AreEqual("The second paragraph in global dictionary", lsEn.Value);
        }

        [TestMethod]
        [Ignore]
        public void PerformanceTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string>
                {
                    "en",
                    "es",
                    "hu",
                    "zh",
                    "cs"
                },
                AutoLoadResources = true,
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            Localization.AttachLogger(new NullLoggerFactory());

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 50; j++)
                {
                    var ls = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst");
                    var lsQQ = Localization.Translator.Translate(LocTranslationSource.File, "text-qq-odst");
                    var lsEn = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst", new CultureInfo("en"));

                    var ls2 = Localization.Translator.Translate(LocTranslationSource.File, "text-1-odst");
                    var ls2QQ = Localization.Translator.Translate(LocTranslationSource.File, "q");
                    var ls2En = Localization.Translator.Translate(LocTranslationSource.File, "text-5-odst", new CultureInfo("en"));
                }
            }

            sw.Stop();
            Debug.WriteLine("300 000 translations in " + sw.ElapsedMilliseconds + " miliseconds");
        }

        [TestMethod]
        public void AutoOffInitTest()
        {
            LibDeInit();

            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string>
                {
                    "en",
                    "es",
                    "hu",
                    "zh",
                    "cs"
                },
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString(),
                AutoLoadResources = false,
                FirstAutoTranslateResource = LocTranslationSource.File.ToString()
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            var resA = CoreLibrary.Translator.Translator.Translate("ahoj");
            Assert.IsTrue(resA.ResourceNotFound);
        }
    }
}
