using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Translator
{
    [TestClass]
    public class TranslatorTest
    {
        [TestInitialize]
        public void LibInit()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"localization";
            configuration.DbPassword = @"SA";
            configuration.DbSource = @"localhost";
            configuration.DbUser = @"SA";
            configuration.DefaultCulture = "cs";
            configuration.SupportedCultures = new List<string>() {"en", "hu", "zh"};
            configuration.TranslationFallbackMode = TranslateFallbackMode.EmptyString.ToString();
            configuration.AutoLoadResources = true;
            configuration.FirstAutoTranslateResource = EnLocalizationResource.File.ToString();
            
            IConfiguration libConfiguration = new LocalizationConfiguration(configuration);
            
            
            Localization.Init(libConfiguration);            
        }

        [TestCleanup]
        public void LibDeInit()
        {
            //FileLocalization.LibDeinit();
        }

        [TestMethod]
        public void TranslatePluralizationTest()
        {
            string[] expectedStrings =
            {
                "let", //-10
                "let", //-9
                "let", //-8
                "let", //-7
                "let", //-6
                "let", //-5
                "roky", //-4
                "roky", //-3
                "roky", //-2
                "rok", //-1
                "let", //0
                "rok", //1
                "roky", //2
                "roky", //3
                "roky", //4
                "let", //5
                "let", //6
                "let", //7
                "let", //8
                "let", //9
                "let", //10
            };

            for (int i = 0; i < 21; i++)
            {
                int pluralizationNum = i - 10;
                LocalizedString lsA = CoreLibrary.Translator.Translator.TranslatePluralization("klíč-stringu", pluralizationNum);

                Assert.AreEqual(expectedStrings[i], lsA);

            }
        }

    }
}