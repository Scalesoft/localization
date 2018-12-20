using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Resolver;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Translator
{
    [TestClass]
    public class ConcurrentTranslateTest
    {
        private FileLocalizationManager m_fileLocalizationManager;
        private FileDictionaryManager m_dictionaryManager;

        [TestInitialize]
        public void Init()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                   new CultureInfo("en"),
                   new CultureInfo("cs"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key
            };

            m_dictionaryManager = new FileDictionaryManager(localizationConfiguration, JsonDictionaryFactory.FactoryInstance);

            var fallbackCultureResolver = new FallbackCultureResolver(localizationConfiguration);
            m_fileLocalizationManager = new FileLocalizationManager(
                localizationConfiguration, m_dictionaryManager, fallbackCultureResolver
            );
        }

        [TestMethod]
        public void TranslateConcurrentlyTest()
        {
            var availableKeys = new[]
            {
                "cancel",
                "retry",
                "delete",
                "close",
                "yes",
                "no",
                "ok",
            };

            Parallel.For(0, 10000, iteration =>
            {
                var key = availableKeys[iteration % availableKeys.Length];
                m_fileLocalizationManager.Translate(key);
            });
        }

        [TestMethod]
        public void EnumerateDictionariesTest()
        {
            Parallel.For(0, 2000, iteration =>
            {
                var dictionary = m_dictionaryManager.GetDictionary();
                foreach (var keyValuePair in dictionary)
                {
                    Assert.IsNotNull(keyValuePair);
                }
            });
        }
    }
}
