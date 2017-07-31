using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Localization.CoreLibrary.Manager.Impl;
using Localization.CoreLibrary.Util;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Manager
{
    [TestClass]
    public class DictionaryManagerTest
    {
        [TestInitialize]
        public void Init()
        {
            
        }

        [TestMethod]
        public void CultureSupportTest()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            DictionaryManager dictionaryManager = new DictionaryManager(localizationConfiguration);

            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("cs")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("en")));
            Assert.AreEqual(true, dictionaryManager.IsCultureSupported(new CultureInfo("es")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("zh")));
            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(new CultureInfo("en-gb")));

            Assert.AreEqual(false, dictionaryManager.IsCultureSupported(null));
        }

        [TestMethod]
        public void Test()
        {
            LocalizationConfiguration.Configuration configuration = new LocalizationConfiguration.Configuration();
            configuration.BasePath = @"local";
            configuration.DefaultCulture = @"cs";
            configuration.SupportedCultures = new List<string> { "en", "es" };
            configuration.DbSource = @"cosi://sql-source";
            configuration.DbUser = "SA";
            configuration.DbPassword = "SA";

            IConfiguration localizationConfiguration = new LocalizationConfiguration(configuration);

            DictionaryManager dictionaryManager = new DictionaryManager(localizationConfiguration);
            dictionaryManager.LoadAndCheck();

            IEnumerable<LocalizedString> dictionary = dictionaryManager.GetDictionary(new CultureInfo("cs"), "slovniky");
            IEnumerator<LocalizedString> dictionaryEnumerator = dictionary.GetEnumerator();

            while (dictionaryEnumerator.MoveNext())
            {
                Debug.WriteLine(dictionaryEnumerator.Current.Name + "=" + dictionaryEnumerator.Current.Value);
            }


            dictionaryEnumerator.Dispose();
        }

    }
}