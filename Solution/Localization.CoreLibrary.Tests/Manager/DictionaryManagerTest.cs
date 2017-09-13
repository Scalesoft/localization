using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Dictionary.Impl;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class DictionaryManagerTest
    {
        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public void CultureSupportTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            FileDictionaryManager dictionaryManager = new FileDictionaryManager(localizationConfiguration);

            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("cs")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("en")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("es")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("zh")));
            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("en-gb")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(null));
        }

        [TestMethod]
        public void TreeTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localizationTree";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "en-US", "en-GB", "en-CA", "es-MX", "es-US"};
            configuration.TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString();

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            FileDictionaryManager dictionaryManager = new FileDictionaryManager(localizationConfiguration);
            dictionaryManager.BuildDictionaryHierarchyTrees(dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance));

            FileLocalizationManager fileLocalizationManager = new FileLocalizationManager(localizationConfiguration);

            fileLocalizationManager.AddDictionaryManager(dictionaryManager);


            LocalizedString s1 = fileLocalizationManager.Translate("text-1-odst", new CultureInfo("cs"));
            Assert.AreEqual("global cs [text-1-odst]", s1);

            LocalizedString s2 = fileLocalizationManager.Translate("extra-cs-key", new CultureInfo("en-MX"));
            Assert.AreEqual("extra string in CS culture", s2);

            LocalizedString s3 = fileLocalizationManager.Translate("extra-cs-key", new CultureInfo("es-MX"));
            Assert.AreEqual("extra string in CS culture", s3);

            string nopeKey = "nope-key";
            LocalizedString sNope = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual(nopeKey, sNope);

            configuration.TranslationFallbackMode = LocTranslateFallbackMode.EmptyString.ToString();

            LocalizedString sNope2 = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual("", sNope2);

            configuration.TranslationFallbackMode = LocTranslateFallbackMode.Exception.ToString();

            bool exceptionThrown = false;
            try
            {
                LocalizedString sNope3 = fileLocalizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            }
            catch (TranslateException)
            {
                exceptionThrown = true;
            }
             
            Assert.IsTrue(exceptionThrown);
        }


    }
}