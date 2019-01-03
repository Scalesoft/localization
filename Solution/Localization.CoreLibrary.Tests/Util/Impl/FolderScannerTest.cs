using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Localization.CoreLibrary.Configuration;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util.Impl
{
    [TestClass]
    public class FolderScannerTest
    {
        [TestMethod]
        public void CheckScopeResourceFilesTest()
        {
            var configuration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("cs"),
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                },
            };


            var folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance);
            folderScanner.CheckResourceFiles(configuration);
        }

        [TestMethod]
        public void ConstructResourceFileName()
        {
            var configuration = new LocalizationConfiguration
            {
                BasePath = "Localization",
                DefaultCulture = new CultureInfo("cs"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("cs"),
                    new CultureInfo("en"),
                    new CultureInfo("es"),
                },
            };


            var folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance);
            var fileName = folderScanner.ConstructResourceFileName(
                configuration, Path.Combine("Localization", "slovniky"), new CultureInfo("cs")
            );

            Assert.AreEqual(Path.GetFullPath("Localization/slovniky/slovniky.cs.json"), Path.GetFullPath(fileName));
        }
    }
}
