using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class FileDictionaryManagerTest
    {
        [TestMethod]
        public void AutoLoadDictionariesTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "LocalizationTree",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                },
                AutoLoadResources = true,
            };

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration, JsonDictionaryFactory.FactoryInstance);
        }
    }
}
