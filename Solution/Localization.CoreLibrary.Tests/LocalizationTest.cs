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

            Localization.LibInit(localizationConfiguration);  
            
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

            Localization.LibInit(localizationConfiguration);

            bool exceptionThrown = false;
            try
            {
                Localization.LibInit(localizationConfiguration);
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

            Localization.LibInit(localizationConfiguration);

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
            configuration.TranslationFallbackMode = TranslateFallbackMode.Key;

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.LibInit(localizationConfiguration);

            Localization.AttachLogger(new NullLoggerFactory());

            LocalizedString ls = Localization.Translator.Translate("text-2-odst");

            Assert.AreEqual("text-2-odst", ls.Name);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", ls.Value);

            LocalizedString lsQQ = Localization.Translator.Translate("text-QQ-odst");
            Assert.AreEqual("text-QQ-odst", lsQQ);

            LocalizedString lsEn = Localization.Translator.Translate("text-2-odst", new CultureInfo("en"));

            Assert.AreEqual("text-2-odst", lsEn.Name);
            Assert.AreEqual("The second paragraph in global dictionary", lsEn.Value);
        }

        [TestMethod]
       
        public void PerformanceTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es", "hu", "zh" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";
            configuration.TranslationFallbackMode = TranslateFallbackMode.Key;

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.LibInit(localizationConfiguration);

            Localization.AttachLogger(new NullLoggerFactory());

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    LocalizedString ls = Localization.Translator.Translate("text-2-odst");
                    LocalizedString lsQQ = Localization.Translator.Translate("text-qq-odst");
                    LocalizedString lsEn = Localization.Translator.Translate("text-2-odst", new CultureInfo("en"));

                    LocalizedString ls2 = Localization.Translator.Translate("text-1-odst");
                    LocalizedString ls2QQ = Localization.Translator.Translate("q");
                    LocalizedString ls2En = Localization.Translator.Translate("text-5-odst", new CultureInfo("en"));
                }
            }

            sw.Stop();
            Debug.WriteLine("300 000 translations in " + sw.ElapsedMilliseconds + " miliseconds");
        }


    }
}