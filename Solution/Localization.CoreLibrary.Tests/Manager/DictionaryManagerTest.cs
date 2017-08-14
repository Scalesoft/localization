using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Localization.CoreLibrary.Dictionary;
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
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            DictionaryManager dictionaryManager = new DictionaryManager(localizationConfiguration);

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
            configuration.TranslationFallbackMode = TranslateFallbackMode.Key.ToString();

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            DictionaryManager dictionaryManager = new DictionaryManager(localizationConfiguration);
            dictionaryManager.BuildDictionaryHierarchyTrees(dictionaryManager.AutoLoadDictionaries(JsonDictionaryFactory.FactoryInstance));

            LocalizationManager localizationManager = new LocalizationManager(localizationConfiguration);

            localizationManager.AddDictionaryManager(dictionaryManager);


            LocalizedString s1 = localizationManager.Translate("text-1-odst", new CultureInfo("cs"));
            Assert.AreEqual("global cs [text-1-odst]", s1);

            LocalizedString s2 = localizationManager.Translate("extra-cs-key", new CultureInfo("en-MX"));
            Assert.AreEqual("extra string in CS culture", s2);

            LocalizedString s3 = localizationManager.Translate("extra-cs-key", new CultureInfo("es-MX"));
            Assert.AreEqual("extra string in CS culture", s3);

            string nopeKey = "nope-key";
            LocalizedString sNope = localizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual(nopeKey, sNope);

            configuration.TranslationFallbackMode = TranslateFallbackMode.EmptyString.ToString();

            LocalizedString sNope2 = localizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            Assert.AreEqual("", sNope2);

            configuration.TranslationFallbackMode = TranslateFallbackMode.Exception.ToString();

            bool exceptionThrown = false;
            try
            {
                LocalizedString sNope3 = localizationManager.Translate(nopeKey, new CultureInfo("es-MX"));
            }
            catch (TranslateException e)
            {
                exceptionThrown = true;
            }
             
            Assert.IsTrue(exceptionThrown);
        }


    }
}