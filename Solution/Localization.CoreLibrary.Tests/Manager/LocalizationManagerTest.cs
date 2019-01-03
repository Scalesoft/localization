using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Resolver;
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
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key,
                AutoLoadResources = true,
            };

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration, JsonDictionaryFactory.FactoryInstance);

            var fallbackCultureResolver = new FallbackCultureResolver(localizationConfiguration);
            var fileLocalizationManager = new FileLocalizationManager(
                localizationConfiguration, dictionaryManager, fallbackCultureResolver
            );

            var ls = fileLocalizationManager.TranslateFormat("klíč-stringu", new object[] {"pondělí"},
                localizationConfiguration.DefaultCulture, "global");

            Assert.AreEqual("Dnes je pondělí.", ls.Value);
        }

        [TestMethod]
        public void TranslateConstant()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "LocalizationTree",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en")
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key,
                AutoLoadResources = true,
            };

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration, JsonDictionaryFactory.FactoryInstance);

            var fallbackCultureResolver = new FallbackCultureResolver(localizationConfiguration);
            var fileLocalizationManager = new FileLocalizationManager(
                localizationConfiguration, dictionaryManager, fallbackCultureResolver
            );

            var ls = fileLocalizationManager.TranslateConstant("const-date", new CultureInfo("cs"));

            Assert.AreEqual("MMMM dd, yyyy", ls.Value);
            Assert.IsFalse(ls.ResourceNotFound);
        }
    }
}
