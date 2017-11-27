using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Impl;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Dictionary
{
    [TestClass]
    public class LocalizationDictionaryTest
    {
        [TestMethod]
        public void ListTest()
        {
            ILocalizationDictionary localizationDictionary = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");

            Assert.AreEqual(new CultureInfo("cs"), localizationDictionary.CultureInfo());
            Assert.AreEqual("slovniky", localizationDictionary.Scope());

            Dictionary<string,LocalizedString> list = localizationDictionary.List();        
            Assert.AreEqual(8, list.Count);
        }

        [TestMethod]
        public void SameListTestInHashSet()
        {
            ILocalizationDictionary localizationDictionary0 = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");
            ILocalizationDictionary localizationDictionary1 = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");
            ILocalizationDictionary localizationDictionary2 = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");
            ILocalizationDictionary localizationDictionary3 = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");


            HashSet<ILocalizationDictionary> dictionaries = new HashSet<ILocalizationDictionary>();
            dictionaries.Add(localizationDictionary0);
            dictionaries.Add(localizationDictionary1);
            dictionaries.Add(localizationDictionary2);
            dictionaries.Add(localizationDictionary3);

            Assert.AreEqual(1, dictionaries.Count);

            ILocalizationDictionary dic = dictionaries.Last(w => w.CultureInfo()
                .Equals(new CultureInfo("cs")) && w.Scope().Equals("slovniky"));

            Assert.AreEqual(new CultureInfo("cs"), dic.CultureInfo());
            Assert.AreEqual("slovniky", dic.Scope());
            Assert.AreEqual(8, dic.List().Count);
        }

        [TestMethod]
        public void MultipleListTestInHashSet()
        {
            ILocalizationDictionary localizationDictionary0 = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");
            ILocalizationDictionary localizationDictionary1 = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.en.json");
            ILocalizationDictionary localizationDictionary2 = new JsonLocalizationDictionary(@"localization\cs.json");
            ILocalizationDictionary localizationDictionary3 = new JsonLocalizationDictionary(@"localization\en.json");

            HashSet<ILocalizationDictionary> dictionaries = new HashSet<ILocalizationDictionary>();
            dictionaries.Add(localizationDictionary0);
            dictionaries.Add(localizationDictionary1);
            dictionaries.Add(localizationDictionary2);
            dictionaries.Add(localizationDictionary3);

            Assert.AreEqual(4, dictionaries.Count);
        }

        [TestMethod]
        public void DottedKeysTest()
        {
            ILocalizationDictionary localizationDictionary = new JsonLocalizationDictionary(@"localization\obrazky\obrazky.cs.json");
            CultureInfo dictionaryCultureInfo = localizationDictionary.CultureInfo();
            ILocalizationDictionary childLocalizationDictionary = localizationDictionary.ChildDictionary();
            string localizationDictionaryExtension = localizationDictionary.Extension();
            bool localizationDictionaryIsRoot = localizationDictionary.IsRoot;
            bool localizationDictionaryIsLeaf = localizationDictionary.IsLeaf();
            Dictionary<string, LocalizedString> localizedStrings = localizationDictionary.List();
            Dictionary<string, LocalizedString> localizedConstants = localizationDictionary.ListConstants();
            Dictionary<string, PluralizedString> localizedPluralizedStrings = localizationDictionary.ListPlurals();
            ILocalizationDictionary parentLocalizationDictionary = localizationDictionary.ParentDictionary();
            string localizationDictionaryScope = localizationDictionary.Scope();

            Assert.AreEqual("cs", dictionaryCultureInfo.Name);
            Assert.AreEqual(null, childLocalizationDictionary);
            Assert.AreEqual("json", localizationDictionaryExtension);
            Assert.AreEqual(false, localizationDictionaryIsRoot);
            Assert.AreEqual(true, localizationDictionaryIsLeaf);
            Assert.AreEqual(1, localizedStrings.Count);
            Assert.AreEqual(0, localizedConstants.Count);
            Assert.AreEqual(0, localizedPluralizedStrings.Count);
            Assert.AreEqual(null, parentLocalizationDictionary);
            Assert.AreEqual("obrazky", localizationDictionaryScope);

            Assert.AreEqual(true, localizedStrings.ContainsKey("header.jpg"));
            Assert.AreEqual("header.cs.jpg", localizedStrings["header.jpg"]);
        }


    }
}