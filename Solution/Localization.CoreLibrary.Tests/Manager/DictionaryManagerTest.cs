using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class DictionaryManagerTest
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void CultureSupportTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = @"localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es"}
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration);

            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("cs")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("en")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("es")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("zh")));
            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("en-gb")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(null));
        }

        [TestMethod]
        public void TreeTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = @"localizationTree",
                DefaultCulture = "cs",
                SupportedCultures = new List<string>
                {
                    "en",
                    "en-US",
                    "en-GB",
                    "en-CA",
                    "es-MX",
                    "es-US"
                },
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration);
            dictionaryManager.BuildDictionaryHierarchyTrees(dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance));

            var fileLocalizationManager = new FileLocalizationManager(localizationConfiguration);

            fileLocalizationManager.AddDictionaryManager(dictionaryManager);


            var s1 = fileLocalizationManager.Translate("text-1-odst", new CultureInfo("cs"));
            Assert.AreEqual("global cs [text-1-odst]", s1);

            var s2 = fileLocalizationManager.Translate("extra-cs-key", new CultureInfo("en-MX"));
            Assert.AreEqual("extra string in CS culture", s2);

            var s3 = fileLocalizationManager.Translate("extra-cs-key", new CultureInfo("es-MX"));
            Assert.AreEqual("extra string in CS culture", s3);

            var nopeKey = "nope-key";
            var sNope = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual(nopeKey, sNope);

            configuration.TranslationFallbackMode = LocTranslateFallbackMode.EmptyString.ToString();

            var sNope2 = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual("", sNope2);

            configuration.TranslationFallbackMode = LocTranslateFallbackMode.Exception.ToString();

            var exceptionThrown = false;
            try
            {
                var sNope3 = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            }
            catch (TranslateException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}
