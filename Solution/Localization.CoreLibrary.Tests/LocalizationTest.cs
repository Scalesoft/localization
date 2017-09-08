using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests
{
    [TestClass]
    public class LocalizationTest
    {
        [TestCleanup]
        public void LibDeInit()
        {
            Localization.LibDeinit();
        }

        [TestMethod]
        public void InitLibTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> {"en", "es"};
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);  
            
            Assert.IsNotNull(Localization.Dictionary);
            Assert.IsNotNull(Localization.Translator);
        }

        [TestMethod]
        public void DoubleInitLibTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            bool exceptionThrown = false;
            try
            {
                Localization.Init(localizationConfiguration);
            }
            catch (LocalizationLibraryException e)
            {
                exceptionThrown = true;
            }
            if (exceptionThrown)
            {
                Assert.IsTrue(exceptionThrown);
            }         
        }

        [TestMethod]
        public void AttachLoggerTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddDebug();

            Localization.AttachLogger(loggerFactory);

            ILogger logger = LogProvider.GetCurrentClassLogger();

            logger.LogWarning("warning LOG test");
            logger.LogCritical("critical LOG test");
        }

        [TestMethod]
        public void TranslateBasic()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString();
            configuration.AutoLoadResources = true;

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);
            Localization.Init(localizationConfiguration);

            //Localization.Init("localization.json.config");

            Localization.AttachLogger(new NullLoggerFactory());

            LocalizedString ls = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst");

            Assert.AreEqual("text-2-odst", ls.Name);
            Assert.IsFalse(ls.ResourceNotFound);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", ls.Value);

            LocalizedString lsQQ = Localization.Translator.Translate(LocTranslationSource.File, "text-QQ-odst");
            Assert.AreEqual("text-QQ-odst", lsQQ);

            LocalizedString lsEn = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst", new CultureInfo("en"));

            Assert.AreEqual("text-2-odst", lsEn.Name);
            Assert.AreEqual("The second paragraph in global dictionary", lsEn.Value);
        }

        [TestMethod]   
        [Ignore]
        public void PerformanceTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es", "hu", "zh" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.AutoLoadResources = true;
            configuration.TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString();

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            Localization.AttachLogger(new NullLoggerFactory());

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    LocalizedString ls = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst");
                    LocalizedString lsQQ = Localization.Translator.Translate(LocTranslationSource.File, "text-qq-odst");
                    LocalizedString lsEn = Localization.Translator.Translate(LocTranslationSource.File, "text-2-odst", new CultureInfo("en"));

                    LocalizedString ls2 = Localization.Translator.Translate(LocTranslationSource.File, "text-1-odst");
                    LocalizedString ls2QQ = Localization.Translator.Translate(LocTranslationSource.File, "q");
                    LocalizedString ls2En = Localization.Translator.Translate(LocTranslationSource.File, "text-5-odst", new CultureInfo("en"));
                }
            }

            sw.Stop();
            Debug.WriteLine("300 000 translations in " + sw.ElapsedMilliseconds + " miliseconds");
        }

        [TestMethod]
        public void AutoOffInitTest()
        {
            LibDeInit();

            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es", "hu", "zh" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.TranslationFallbackMode = LocTranslateFallbackMode.Key.ToString();
            configuration.AutoLoadResources = false;
            configuration.FirstAutoTranslateResource = LocTranslationSource.File.ToString();

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.Init(localizationConfiguration);

            LocalizedString resA = CoreLibrary.Translator.Translator.Translate("ahoj");
            Assert.IsTrue(resA.ResourceNotFound);

        }
    }
}