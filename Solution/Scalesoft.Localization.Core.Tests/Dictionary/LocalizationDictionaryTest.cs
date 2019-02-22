using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Dictionary.Factory;

namespace Scalesoft.Localization.Core.Tests.Dictionary
{
    [TestClass]
    public class LocalizationDictionaryTest
    {
        [TestMethod]
        public void ListTest()
        {
            var dictionaryFactory = new JsonDictionaryFactory();
            var localizationDictionary = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.cs.json");

            Assert.AreEqual(new CultureInfo("cs"), localizationDictionary.CultureInfo());
            Assert.AreEqual("slovniky", localizationDictionary.Scope());

            var list = localizationDictionary.List();
            Assert.AreEqual(13, list.Count);
        }

        [TestMethod]
        public void SameListTestInHashSet()
        {
            var dictionaryFactory = new JsonDictionaryFactory();
            var localizationDictionary0 = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.cs.json");
            var localizationDictionary1 = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.cs.json");
            var localizationDictionary2 = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.cs.json");
            var localizationDictionary3 = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.cs.json");


            var dictionaries = new HashSet<ILocalizationDictionary>
            {
                localizationDictionary0,
                localizationDictionary1,
                localizationDictionary2,
                localizationDictionary3
            };

            Assert.AreEqual(1, dictionaries.Count);

            var dic = dictionaries.Last(w => w.CultureInfo()
                                                 .Equals(new CultureInfo("cs")) && w.Scope().Equals("slovniky"));

            Assert.AreEqual(new CultureInfo("cs"), dic.CultureInfo());
            Assert.AreEqual("slovniky", dic.Scope());
            Assert.AreEqual(13, dic.List().Count);
        }

        [TestMethod]
        public void MultipleListTestInHashSet()
        {
            var dictionaryFactory = new JsonDictionaryFactory();
            var localizationDictionary0 = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.cs.json");
            var localizationDictionary1 = dictionaryFactory.CreateDictionary("Localization/slovniky/slovniky.en.json");
            var localizationDictionary2 = dictionaryFactory.CreateDictionary("Localization/cs.json");
            var localizationDictionary3 = dictionaryFactory.CreateDictionary("Localization/en.json");

            var dictionaries = new HashSet<ILocalizationDictionary>
            {
                localizationDictionary0,
                localizationDictionary1,
                localizationDictionary2,
                localizationDictionary3
            };

            Assert.AreEqual(4, dictionaries.Count);
        }

        [TestMethod]
        public void DottedKeysTest()
        {
            var dictionaryFactory = new JsonDictionaryFactory();
            var localizationDictionary = dictionaryFactory.CreateDictionary("Localization/obrazky/obrazky.cs.json");
            var dictionaryCultureInfo = localizationDictionary.CultureInfo();
            var childLocalizationDictionary = localizationDictionary.ChildDictionaries;
            var localizationDictionaryExtension = localizationDictionary.Extensions();
            var localizationDictionaryIsRoot = localizationDictionary.IsRoot;
            var localizationDictionaryIsLeaf = localizationDictionary.IsLeaf();
            var localizedStrings = localizationDictionary.List();
            var localizedConstants = localizationDictionary.ListConstants();
            var localizedPluralizedStrings = localizationDictionary.ListPlurals();
            var parentLocalizationDictionary = localizationDictionary.ParentDictionary();
            var localizationDictionaryScope = localizationDictionary.Scope();

            Assert.AreEqual("cs", dictionaryCultureInfo.Name);
            Assert.AreEqual(0, childLocalizationDictionary.Count);
            Assert.AreEqual("json", localizationDictionaryExtension[0]);
            Assert.AreEqual("json5", localizationDictionaryExtension[1]);
            Assert.IsTrue(localizationDictionaryIsRoot);
            Assert.IsTrue(localizationDictionaryIsLeaf);
            Assert.AreEqual(1, localizedStrings.Count);
            Assert.AreEqual(0, localizedConstants.Count);
            Assert.AreEqual(0, localizedPluralizedStrings.Count);
            Assert.IsNull(parentLocalizationDictionary);
            Assert.AreEqual("obrazky", localizationDictionaryScope);

            Assert.IsTrue(localizedStrings.ContainsKey("header.jpg"));
            Assert.AreEqual("header.cs.jpg", localizedStrings["header.jpg"]);
        }
    }
}