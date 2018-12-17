using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class FileDictionaryManagerTest
    {
        [TestMethod]
        public void AutoLoadDictionariesTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "LocalizationTree",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "es"}
            };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration);

            dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
        }
    }
}
