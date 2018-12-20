using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Translator
{
    [TestClass]
    public class TranslatorTest
    {
        private IAutoLocalizationManager m_dictionaryManager;

        [TestInitialize]
        public void LibInit()
        {
            var configuration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("en"),
                    new CultureInfo("cs"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.EmptyString,
                AutoLoadResources = true,
                FirstAutoTranslateResource = LocLocalizationResource.File
            };

            Localization.Init(configuration);

            m_dictionaryManager = Localization.Instance();

            Localization.LibDeinit();
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

            for (var i = 0; i < 21; i++)
            {
                var pluralizationNum = i - 10;
                var lsA = m_dictionaryManager.TranslatePluralization(
                    LocTranslationSource.Auto, "klíč-stringu", pluralizationNum, null,
                    "slovniky"
                );

                Assert.AreEqual(expectedStrings[i], lsA);
            }
        }
    }
}
