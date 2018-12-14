using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util.Impl
{
    [TestClass]
    public class FolderScannerTest
    {
        [TestMethod]
        public void CheckScopeResourceFilesTest()
        {
            var product =
                new LocalizationConfiguration.Configuration
                {
                    BasePath = "localization",
                    DefaultCulture = "cs",
                    SupportedCultures = new List<string> {"cs", "en", "es"},
                };

            Localization.AttachLogger(new LoggerFactory());

            IConfiguration configuration = new LocalizationConfiguration(product);

            var folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance);
            folderScanner.CheckResourceFiles(configuration);
        }

        [TestMethod]
        public void ConstructResourceFileName()
        {
            var product =
                new LocalizationConfiguration.Configuration
                {
                    BasePath = "localization",
                    DefaultCulture = "cs",
                    SupportedCultures = new List<string> {"cs", "en", "es"},
                };

            Localization.AttachLogger(new LoggerFactory());

            IConfiguration configuration = new LocalizationConfiguration(product);

            var folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance);
            var fileName =
                folderScanner.ConstructResourceFileName(configuration, Path.Combine("localization", "slovniky"), new CultureInfo("cs"));


            Assert.AreEqual(@"localization\slovniky\slovniky.cs.json", fileName);
        }
    }
}
