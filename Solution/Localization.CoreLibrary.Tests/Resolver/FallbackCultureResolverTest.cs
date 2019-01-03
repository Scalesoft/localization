using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Resolver;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Resolver
{
    [TestClass]
    public class FallbackCultureResolverTest
    {
        [TestMethod]
        public void TranslateFormatTest()
        {
            var localizationConfiguration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("cs-CZ"),
                    new CultureInfo("en"),
                    new CultureInfo("en-US"),
                    new CultureInfo("es"),
                },
                TranslateFallbackMode = LocTranslateFallbackMode.Key
            };

            var fallbackCultureResolver = new FallbackCultureResolver(localizationConfiguration);

            var cultureCs = new CultureInfo("cs");
            var cultureCsCz = new CultureInfo("cs-CZ");
            var cultureEn = new CultureInfo("en");
            var cultureEnUs = new CultureInfo("en-US");
            var cultureEs = new CultureInfo("es");
            var cultureDe = new CultureInfo("de");

            Assert.AreEqual(null, fallbackCultureResolver.FallbackCulture(cultureCs));
            Assert.AreEqual(cultureCs, fallbackCultureResolver.FallbackCulture(cultureCsCz));
            Assert.AreEqual(cultureCs, fallbackCultureResolver.FallbackCulture(cultureEn));
            Assert.AreEqual(cultureEn, fallbackCultureResolver.FallbackCulture(cultureEnUs));
            Assert.AreEqual(cultureCs, fallbackCultureResolver.FallbackCulture(cultureEs));
            Assert.AreEqual(null, fallbackCultureResolver.FallbackCulture(cultureDe));
        }
    }
}
