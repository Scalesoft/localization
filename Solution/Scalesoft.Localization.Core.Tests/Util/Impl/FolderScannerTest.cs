using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary.Factory;
using Scalesoft.Localization.Core.Util.Impl;

namespace Scalesoft.Localization.Core.Tests.Util.Impl
{
    [TestClass]
    public class FolderScannerTest
    {
        [TestMethod]
        public void CheckResourceFilesCountTest()
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


            var folderScanner = new FolderScanner(JsonDictionaryFactory.FactoryInstance, configuration);
            Assert.AreEqual(15, folderScanner.GetAllDictionaryFullpaths(configuration.BasePath).Count);
        }
    }
}
