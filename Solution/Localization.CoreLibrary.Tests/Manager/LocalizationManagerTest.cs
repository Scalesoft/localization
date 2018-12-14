using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class LocalizationManagerTest
    {
        [TestMethod]
        public void TranslateFormatTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es"},
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration);
            var loadedDictionaries = dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
            dictionaryManager.BuildDictionaryHierarchyTrees(loadedDictionaries);

            var fileLocalizationManager = new FileLocalizationManager(localizationConfiguration);
            fileLocalizationManager.AddDictionaryManager(dictionaryManager);

            var ls = fileLocalizationManager.TranslateFormat("klíč-stringu", new[] {"pondělí"},
                new CultureInfo(configuration.DefaultCulture), "global");

            Assert.AreEqual("Dnes je pondělí.", ls.Value);
        }

        [TestMethod]
        public void TranslateConstant()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "localizationTree",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en"},
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration);
            var loadedDictionaries = dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
            dictionaryManager.BuildDictionaryHierarchyTrees(loadedDictionaries);

            var fileLocalizationManager = new FileLocalizationManager(localizationConfiguration);
            fileLocalizationManager.AddDictionaryManager(dictionaryManager);

            var ls = fileLocalizationManager.TranslateConstant("const-date", new CultureInfo("cs"));

            Assert.AreEqual("MMMM dd, yyyy", ls.Value);
            Assert.IsFalse(ls.ResourceNotFound);
        }
    }
}
