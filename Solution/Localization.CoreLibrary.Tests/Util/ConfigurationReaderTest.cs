using System.Globalization;
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
            var configurationReader = new JsonConfigurationReader("localization.json.config");
            var configuration = configurationReader.ReadConfiguration();

            Assert.AreEqual("localization", configuration.BasePath());

            Assert.AreEqual(3, configuration.SupportedCultures().Count);
            Assert.AreEqual(new CultureInfo("cs"), configuration.SupportedCultures()[0]);
            Assert.AreEqual(new CultureInfo("en"), configuration.SupportedCultures()[1]);
            Assert.AreEqual(new CultureInfo("es"), configuration.SupportedCultures()[2]);

            Assert.AreEqual(new CultureInfo("cs"), configuration.DefaultCulture());
        }
    }
}
