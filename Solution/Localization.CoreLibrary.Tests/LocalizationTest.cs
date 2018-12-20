using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Models;
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
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                    new CultureInfo("cs"),
                }
            };

            Localization.Init(localizationConfiguration);

            var instance = Localization.Instance();

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void DoubleInitLibTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                    new CultureInfo("cs"),
                }
            };

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
        public void TranslateBasic()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                    new CultureInfo("cs"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key,
                AutoLoadResources = true
            };

            Localization.Init(localizationConfiguration);

            var instance = Localization.Instance();

            //Localization.Init("localization.json.config");

            var ls = instance.Translate(LocTranslationSource.File, "text-2-odst");

            Assert.AreEqual("text-2-odst", ls.Name);
            Assert.IsFalse(ls.ResourceNotFound);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", ls.Value);

            var lsQQ = instance.Translate(LocTranslationSource.File, "text-QQ-odst");
            Assert.AreEqual("text-QQ-odst", lsQQ);

            var lsEn = instance.Translate(LocTranslationSource.File, "text-2-odst", new CultureInfo("en"));

            Assert.AreEqual("text-2-odst", lsEn.Name);
            Assert.AreEqual("The second paragraph in global dictionary", lsEn.Value);
        }

        [TestMethod]
        public void PerformanceTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                    new CultureInfo("hu"),
                    new CultureInfo("zh"),
                    new CultureInfo("cs"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key,
                AutoLoadResources = true
            };

            Localization.Init(localizationConfiguration);

            var instance = Localization.Instance();

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 50; j++)
                {
                    var ls = instance.Translate(LocTranslationSource.File, "text-2-odst");
                    var lsQQ = instance.Translate(LocTranslationSource.File, "text-qq-odst");
                    var lsEn = instance.Translate(LocTranslationSource.File, "text-2-odst", new CultureInfo("en"));

                    var ls2 = instance.Translate(LocTranslationSource.File, "text-1-odst");
                    var ls2QQ = instance.Translate(LocTranslationSource.File, "q");
                    var ls2En = instance.Translate(LocTranslationSource.File, "text-5-odst", new CultureInfo("en"));
                }
            }

            sw.Stop();
            Debug.WriteLine("300 000 translations in " + sw.ElapsedMilliseconds + " miliseconds");
        }

        [TestMethod]
        public void AutoOffInitTest()
        {
            LibDeInit();

            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                    new CultureInfo("hu"),
                    new CultureInfo("zh"),
                    new CultureInfo("cs"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key,
                FirstAutoTranslateResource = LocLocalizationResource.File,
                AutoLoadResources = false
            };

            Localization.Init(localizationConfiguration);

            var instance = Localization.Instance();

            var resA = instance.Translate(LocTranslationSource.Auto, "ahoj");
            Assert.IsTrue(resA.ResourceNotFound);
        }
    }
}
