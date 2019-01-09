using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Tests
{
    [TestClass]
    public class LocalizationTest
    {
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

            var instance = new LocalizationLib(localizationConfiguration);

            Assert.IsNotNull(instance);
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

            var instance = new LocalizationLib(localizationConfiguration);

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

            var instance = new LocalizationLib(localizationConfiguration);

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

            var instance = new LocalizationLib(localizationConfiguration);

            var resA = instance.Translate(LocTranslationSource.Auto, "ahoj");
            Assert.IsTrue(resA.ResourceNotFound);
        }
    }
}
