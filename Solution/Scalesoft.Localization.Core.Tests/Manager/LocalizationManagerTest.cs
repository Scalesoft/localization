using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary.Factory;
using Scalesoft.Localization.Core.Manager.Impl;
using Scalesoft.Localization.Core.Resolver;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.Core.Tests.Manager
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
