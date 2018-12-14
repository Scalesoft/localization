﻿using System.Globalization;
using Localization.CoreLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util
{
    [TestClass]
    public class LocalizationConfigurationTest
    {
        [TestMethod]
        public void CheckConfData()
        {
            var supportedCultures = new[]
            {
                "cs",
                "en",
                "es"
            };

            var defaultCulture = new CultureInfo("cs");

            var localizationConfiguration =
                new LocalizationConfiguration()
                    .SetBasePath("/localizationfolder")
                    .SetSupportedCultures(supportedCultures)
                    .SetDefaultCulture(defaultCulture);

            IConfiguration configuration = localizationConfiguration;

            Assert.AreEqual("/localizationfolder", configuration.BasePath());

            var takenCulture = configuration.SupportedCultures();
            Assert.AreEqual(3, takenCulture.Count);
            Assert.AreEqual(new CultureInfo("cs"), takenCulture[0]);
            Assert.AreEqual(new CultureInfo("en"), takenCulture[1]);
            Assert.AreEqual(new CultureInfo("es"), takenCulture[2]);

            Assert.AreEqual(new CultureInfo("cs"), configuration.DefaultCulture());
        }
    }
}
