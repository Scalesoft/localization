using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Impl;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class LocalizationManagerTest
    {
        [TestMethod]
        public void TranslateFormatTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.TranslationFallbackMode = TranslateFallbackMode.Key.ToString();

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            DictionaryManager dictionaryManager = new DictionaryManager(localizationConfiguration);
            ILocalizationDictionary[] loadedDictionaries = dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
            dictionaryManager.BuildDictionaryHierarchyTrees(loadedDictionaries);

            LocalizationManager localizationManager = new LocalizationManager(localizationConfiguration);
            localizationManager.AddDictionaryManager(dictionaryManager);

            LocalizedString ls = localizationManager.TranslateFormat("klíč-stringu", new []{"pondělí"}, new CultureInfo(configuration.DefaultCulture), "global");

            Assert.AreEqual("Dnes je pondělí.", ls.Value);
        }

        [TestMethod]
        public void TranslateConstant()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localizationTree";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.TranslationFallbackMode = TranslateFallbackMode.Key.ToString();

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            DictionaryManager dictionaryManager = new DictionaryManager(localizationConfiguration);
            ILocalizationDictionary[] loadedDictionaries = dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance);
            dictionaryManager.BuildDictionaryHierarchyTrees(loadedDictionaries);

            LocalizationManager localizationManager = new LocalizationManager(localizationConfiguration);
            localizationManager.AddDictionaryManager(dictionaryManager);

            LocalizedString ls = localizationManager.TranslateConstant("const-date", new CultureInfo("cs"));

            Assert.AreEqual("MMMM dd, yyyy", ls.Value);
            Assert.IsFalse(ls.ResourceNotFound);
        }

    }
}