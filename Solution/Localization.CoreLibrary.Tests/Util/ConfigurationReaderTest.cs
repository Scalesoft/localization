using System.Globalization;
using System.IO;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util
{
    [TestClass]
    public class ConfigurationReaderTest
    {
        [TestMethod]
        public void TestConfigurationReader()
        {
            var configurationReader = new JsonConfigurationReader("localization.config.json");
            var configuration = configurationReader.ReadConfiguration();

            Assert.AreEqual("Localization", configuration.BasePath);

            Assert.AreEqual(3, configuration.SupportedCultures.Count);
            Assert.AreEqual(new CultureInfo("cs"), configuration.SupportedCultures[0]);
            Assert.AreEqual(new CultureInfo("en"), configuration.SupportedCultures[1]);
            Assert.AreEqual(new CultureInfo("es"), configuration.SupportedCultures[2]);

            Assert.AreEqual(new CultureInfo("cs"), configuration.DefaultCulture);
        }
    }
}
