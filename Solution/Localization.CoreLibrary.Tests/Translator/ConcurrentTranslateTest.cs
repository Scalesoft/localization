using System.Collections.Generic;
using System.Threading.Tasks;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Manager.Impl;
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
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = "Localization",
                DefaultCulture = "cs",
                SupportedCultures = new List<string> {"en", "cs"},
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            var localizationConfiguration = new LocalizationConfiguration(configuration);

            m_dictionaryManager = new FileDictionaryManager(localizationConfiguration);
            var loadedDictionaries = m_dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
            m_dictionaryManager.BuildDictionaryHierarchyTrees(loadedDictionaries);

            m_fileLocalizationManager = new FileLocalizationManager(localizationConfiguration);
            m_fileLocalizationManager.AddDictionaryManager(m_dictionaryManager);
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
