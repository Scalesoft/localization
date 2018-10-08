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
        [TestMethod]
        public void TranslateConcurrentlyTest()
        {
            var configuration = new LocalizationConfiguration.Configuration
            {
                BasePath = @"localization",
                DefaultCulture = @"cs",
                SupportedCultures = new List<string> {"en", "cs"},
                TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString()
            };

            var localizationConfiguration = new LocalizationConfiguration(configuration);

            var dictionaryManager = new FileDictionaryManager(localizationConfiguration);
            var loadedDictionaries = dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
            dictionaryManager.BuildDictionaryHierarchyTrees(loadedDictionaries);

            var fileLocalizationManager = new FileLocalizationManager(localizationConfiguration);
            fileLocalizationManager.AddDictionaryManager(dictionaryManager);


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
                fileLocalizationManager.Translate(key);
            });
        }
    }
}
