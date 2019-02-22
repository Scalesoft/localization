using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary.Factory;
using Scalesoft.Localization.Core.Dictionary.Impl;
using Scalesoft.Localization.Core.Util.Impl;

namespace Scalesoft.Localization.Core.Tests.Util.Impl
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
                configuration, Path.Combine("Localization", "slovniky"), new CultureInfo("cs"), JsonLocalizationDictionary.JsonExtension
            );

            Assert.AreEqual(Path.GetFullPath("Localization/slovniky/slovniky.cs.json"), Path.GetFullPath(fileName));
        }
    }
}
