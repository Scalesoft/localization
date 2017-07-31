using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests
{
    [TestClass]
    public class LocalizationTest
    {
        [TestCleanup]
        public void LibInit()
        {
            Localization.LibDeinit();
        }

        [TestMethod]
        public void InitLibTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"";
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
            configuration.BasePath = @"";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.LibInit(localizationConfiguration);


            //Haze porad exception. Coz ma, ale chci to zachytit
            //Action libInitActionDelegate = delegate { Localization.LibInit(localizationConfiguration); };
            //Assert.ThrowsException<Exception>(libInitActionDelegate);
        }

        [TestMethod]
        public void AttachLoggerTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.LibInit(localizationConfiguration);

            Localization.AttachLogger(new NullLoggerFactory());
        }

        [TestMethod]
        public void TranslateBasic()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            Localization.LibInit(localizationConfiguration);

            Localization.AttachLogger(new NullLoggerFactory());

            LocalizedString ls = Localization.Translator.Translate("text-2-odst");

            Assert.AreEqual("text-2-odst", ls.Name);
            Assert.AreEqual("Druhý odstavec v globálním slovníku", ls.Value);

            LocalizedString lsQQ = Localization.Translator.Translate("text-QQ-odst");
            Assert.AreEqual("text-QQ-odst", lsQQ);

            LocalizedString lsEn = Localization.Translator.Translate("text-2-odst", new CultureInfo("en-US"));

            Assert.AreEqual("text-2-odst", lsEn.Name);
            Assert.AreEqual("The second paragraph in global dictionary", lsEn.Value);
        }

    }
}