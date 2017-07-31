using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util
{
    [TestClass]
    public class LocalizationConfigurationTest
    {
        [TestMethod]
        public void CheckConfData()
        {
            string[] supportedCultures = new string[3];
            supportedCultures[0] = "cs";
            supportedCultures[1] = "en";
            supportedCultures[2] = "es";

            CultureInfo defaultCulture = new CultureInfo("cs");


            LocalizationConfiguration localizationConfiguration =
                new LocalizationConfiguration()
                    .SetBasePath("/localizationfolder")
                    .SetSupportedCultures(supportedCultures)
                    .SetDefaultCulture(defaultCulture)
                    .SetDbSource("mysql://localhost:3306/texty")
                    .SetDbUser("SA")
                    .SetDbPass("SA");

            IConfiguration configuration = localizationConfiguration;


            Assert.AreEqual("/localizationfolder", configuration.BasePath());

            IList<CultureInfo> takenCulture = configuration.SupportedCultures();
            Assert.AreEqual(3, takenCulture.Count);
            Assert.AreEqual(new CultureInfo("cs"), takenCulture[0]);
            Assert.AreEqual(new CultureInfo("en"), takenCulture[1]);
            Assert.AreEqual(new CultureInfo("es"), takenCulture[2]);

            Assert.AreEqual(new CultureInfo("cs"), configuration.DefaultCulture());


            Assert.AreEqual("mysql://localhost:3306/texty", configuration.DbSource());
            Assert.AreEqual("SA", configuration.DbUser());
            Assert.AreEqual("SA", configuration.DbPassword());
        }


    }
}