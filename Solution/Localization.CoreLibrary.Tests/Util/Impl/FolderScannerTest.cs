using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Localization.CoreLibrary.Dictionary.Impl;
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
            LocalizationConfiguration.Configuration product =
                new LocalizationConfiguration.Configuration
                {
                    BasePath = @"localization",
                    DefaultCulture = "cs",
                    SupportedCultures = new List<string> {"cs", "en", "es"},
                    DbSource = "vla",
                    DbUser = "bla",
                    DbPassword = "kla"
                };

            Localization.AttachLogger(new LoggerFactory());

            IConfiguration configuration = new LocalizationConfiguration(product);

            FolderScanner folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance);
            folderScanner.CheckResourceFiles(configuration);
        }

        [TestMethod]
        public void ConstructResourceFileName()
        {
            LocalizationConfiguration.Configuration product =
                new LocalizationConfiguration.Configuration
                {
                    BasePath = @"localization",
                    DefaultCulture = "cs",
                    SupportedCultures = new List<string> { "cs", "en", "es" },
                    DbSource = "vla",
                    DbUser = "bla",
                    DbPassword = "kla"
                };

            Localization.AttachLogger(new LoggerFactory());

            IConfiguration configuration = new LocalizationConfiguration(product);

            FolderScanner folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance);
            string fileName = folderScanner.ConstructResourceFileName(configuration, Path.Combine("localization", "slovniky"), new CultureInfo("cs"));


            Assert.AreEqual("localization\\slovniky\\slovniky.cs.json", fileName);
        }
    }
}