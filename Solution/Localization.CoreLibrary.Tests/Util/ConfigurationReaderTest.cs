using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Localization.CoreLibrary.Tests.Util
{
    [TestClass]
    public class ConfigurationReaderTest
    {
        [TestMethod]
        public void TestConfigurationReader()
        {
            LocalizationConfiguration.Configuration product = new LocalizationConfiguration.Configuration();
            product.BasePath = @"path\to\localization";
            product.DefaultCulture = "cs";
            product.SupportedCultures = new List<string>();
            product.SupportedCultures.Add("cs");
            product.SupportedCultures.Add("en");
            product.SupportedCultures.Add("es");

            product.DbSource = "vla";
            product.DbUser = "bla";
            product.DbPassword = "kla";

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            //serializer.Converters.Add(new ToStringJsonConverter());

            using (Stream stream = new FileStream("localization.json.config", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(stream, Encoding.Unicode))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, product);
            }

            JsonConfigurationReader configurationReader = new JsonConfigurationReader("localization.json.config");
            IConfiguration configuration = configurationReader.ReadConfiguration();

            Assert.AreEqual(@"path\to\localization", configuration.BasePath());
            Assert.AreEqual("vla", configuration.DbSource());
            Assert.AreEqual("bla", configuration.DbUser());
            Assert.AreEqual("kla", configuration.DbPassword());

            Assert.AreEqual(3, configuration.SupportedCultures().Count);
            Assert.AreEqual(new CultureInfo("cs"), configuration.SupportedCultures()[0]);
            Assert.AreEqual(new CultureInfo("en"), configuration.SupportedCultures()[1]);
            Assert.AreEqual(new CultureInfo("es"), configuration.SupportedCultures()[2]);

            Assert.AreEqual(new CultureInfo("cs"), configuration.DefaultCulture());
        }

    }
}