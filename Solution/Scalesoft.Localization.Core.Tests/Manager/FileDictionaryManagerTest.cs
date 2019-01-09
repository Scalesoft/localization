using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary.Factory;
using Scalesoft.Localization.Core.Manager.Impl;

namespace Scalesoft.Localization.Core.Tests.Manager
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
