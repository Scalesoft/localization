using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util
{
    [TestClass]
    public class ConfigurationReaderTest
    {
        [TestMethod]
        public void TestConfigurationReader()
        {
            JsonConfigurationReader configurationReader = new JsonConfigurationReader("localization.json.config");
            IConfiguration configuration = configurationReader.ReadConfiguration();

            Assert.AreEqual(@"localization", configuration.BasePath());

            Assert.AreEqual(3, configuration.SupportedCultures().Count);
            Assert.AreEqual(new CultureInfo("cs"), configuration.SupportedCultures()[0]);
            Assert.AreEqual(new CultureInfo("en"), configuration.SupportedCultures()[1]);
            Assert.AreEqual(new CultureInfo("es"), configuration.SupportedCultures()[2]);

            Assert.AreEqual(new CultureInfo("cs"), configuration.DefaultCulture());
        }

    }
}