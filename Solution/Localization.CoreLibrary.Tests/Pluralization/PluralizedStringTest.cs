using System;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Dictionary.Impl;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Localization.CoreLibrary.Tests.Pluralization
{
    [TestClass]
    public class PluralizedStringTest
    {
        [TestMethod]
        public void BuildSimplePluralizedStringTest()
        {
            LocalizedString defaultLocalizedString = new LocalizedString("let", "let");
            PluralizedString psA = new PluralizedString(defaultLocalizedString);

            Assert.AreEqual(defaultLocalizedString, psA.GetPluralizedLocalizedString(0));
        }

        [TestMethod]
        public void ConstructorDefaultStringNullTest()
        {
            bool exceptionThrown = false;
            try
            {
                PluralizedString psA = new PluralizedString(null);
            }
            catch (PluralizedDefaultStringException)
            {
                exceptionThrown = true;
            }
            
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void IntervalCompletePluralizedStringTest()
        {
            LocalizedString defaultLocalizedString = new LocalizedString("let", "let");
            PluralizedString psA = new PluralizedString(defaultLocalizedString);
            psA.Add(new PluralizationInterval(int.MinValue, -5), new LocalizedString("let", "let"));
            psA.Add(new PluralizationInterval(-4, -2), new LocalizedString("let", "roky"));
            psA.Add(new PluralizationInterval(-1, -1), new LocalizedString("let", "rok"));
            psA.Add(new PluralizationInterval(0, 0), new LocalizedString("let", "let"));
            psA.Add(new PluralizationInterval(1, 1), new LocalizedString("let", "rok"));
            psA.Add(new PluralizationInterval(2, 4), new LocalizedString("let", "roky"));
            psA.Add(new PluralizationInterval(5, int.MaxValue), new LocalizedString("let", "let"));


            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(-10));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(-9));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(-8));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(-7));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(-6));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(-5));
            Assert.AreEqual("roky", psA.GetPluralizedLocalizedString(-4));
            Assert.AreEqual("roky", psA.GetPluralizedLocalizedString(-3));
            Assert.AreEqual("roky", psA.GetPluralizedLocalizedString(-2));
            Assert.AreEqual("rok", psA.GetPluralizedLocalizedString(-1));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(0));
            Assert.AreEqual("rok", psA.GetPluralizedLocalizedString(1));
            Assert.AreEqual("roky", psA.GetPluralizedLocalizedString(2));
            Assert.AreEqual("roky", psA.GetPluralizedLocalizedString(3));
            Assert.AreEqual("roky", psA.GetPluralizedLocalizedString(4));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(5));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(6));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(7));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(8));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(9));
            Assert.AreEqual("let", psA.GetPluralizedLocalizedString(10));
        }

        [TestMethod]
        public void OverlapExceptionTest()
        {
            bool exceptionThrown = false;

            LocalizedString defaultLocalizedString = new LocalizedString("let", "let");
            PluralizedString psA = new PluralizedString(defaultLocalizedString);
            psA.Add(new PluralizationInterval(int.MinValue, -5), new LocalizedString("let", "let"));
            psA.Add(new PluralizationInterval(-4, -2), new LocalizedString("let", "roky"));
            psA.Add(new PluralizationInterval(-1, -1), new LocalizedString("let", "rok"));
            psA.Add(new PluralizationInterval(0, 0), new LocalizedString("let", "let"));
            psA.Add(new PluralizationInterval(1, 1), new LocalizedString("let", "rok"));
            psA.Add(new PluralizationInterval(2, 4), new LocalizedString("let", "roky"));
            psA.Add(new PluralizationInterval(5, int.MaxValue), new LocalizedString("let", "let"));

            try
            {
                psA.Add(new PluralizationInterval(1, 2), new LocalizedString("let", "rok"));
            }
            catch (PluralizedStringIntervalOverlapException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void PluralizedDictionaryDottedKeyTest()
        {
            ILocalizationDictionary localizationDictionary = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");
            Dictionary<string, PluralizedString> localizedPluralizedStrings = localizationDictionary.ListPlurals();
            Assert.AreEqual(true, localizedPluralizedStrings.ContainsKey("klíč.stringu"));

            PluralizedString pluralizedString = localizedPluralizedStrings["klíč.stringu"];

            LocalizedString min = pluralizedString.GetPluralizedLocalizedString(Int32.MinValue);
            LocalizedString max = pluralizedString.GetPluralizedLocalizedString(Int32.MaxValue);
            LocalizedString zero = pluralizedString.GetPluralizedLocalizedString(0);
            LocalizedString one = pluralizedString.GetPluralizedLocalizedString(1);
            LocalizedString two = pluralizedString.GetPluralizedLocalizedString(2);
            LocalizedString three = pluralizedString.GetPluralizedLocalizedString(3);
            LocalizedString four = pluralizedString.GetPluralizedLocalizedString(4);
            LocalizedString five = pluralizedString.GetPluralizedLocalizedString(5);
            LocalizedString six = pluralizedString.GetPluralizedLocalizedString(6);

            Assert.AreEqual("let", min.Value);
            Assert.AreEqual(false, min.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", min.Name);

            Assert.AreEqual("let", max.Value);
            Assert.AreEqual(false, max.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", max.Name);

            Assert.AreEqual("let", zero.Value);
            Assert.AreEqual(false, zero.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", zero.Name);

            Assert.AreEqual("rok", one.Value);
            Assert.AreEqual(false, zero.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", zero.Name);

            Assert.AreEqual("roky", two.Value);
            Assert.AreEqual(false, two.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", two.Name);

            Assert.AreEqual("roky", three.Value);
            Assert.AreEqual(false, three.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", three.Name);

            Assert.AreEqual("roky", four.Value);
            Assert.AreEqual(false, four.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", four.Name);

            Assert.AreEqual("let", five.Value);
            Assert.AreEqual(false, five.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", five.Name);

            Assert.AreEqual("let", six.Value);
            Assert.AreEqual(false, six.ResourceNotFound);
            Assert.AreEqual("klíč.stringu", six.Name);

        }


        [TestMethod]
        public void PluralizedDictionaryLoadTest()
        {
            ILocalizationDictionary localizationDictionary = new JsonLocalizationDictionary(@"localization\slovniky\slovniky.cs.json");
            Dictionary<string, PluralizedString> localizedPluralizedStrings = localizationDictionary.ListPlurals();
            Assert.AreEqual(2, localizedPluralizedStrings.Count);

            Assert.AreEqual(true, localizedPluralizedStrings.ContainsKey("klíč-stringu"));
            
            PluralizedString pluralizedString = localizedPluralizedStrings["klíč-stringu"];

            LocalizedString min = pluralizedString.GetPluralizedLocalizedString(Int32.MinValue);
            LocalizedString max = pluralizedString.GetPluralizedLocalizedString(Int32.MaxValue);
            LocalizedString zero = pluralizedString.GetPluralizedLocalizedString(0);
            LocalizedString one = pluralizedString.GetPluralizedLocalizedString(1);
            LocalizedString two = pluralizedString.GetPluralizedLocalizedString(2);
            LocalizedString three = pluralizedString.GetPluralizedLocalizedString(3);
            LocalizedString four = pluralizedString.GetPluralizedLocalizedString(4);
            LocalizedString five = pluralizedString.GetPluralizedLocalizedString(5);
            LocalizedString six = pluralizedString.GetPluralizedLocalizedString(6);

            Assert.AreEqual("let", min.Value);
            Assert.AreEqual(false, min.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", min.Name);

            Assert.AreEqual("let", max.Value);
            Assert.AreEqual(false, max.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", max.Name);

            Assert.AreEqual("let", zero.Value);
            Assert.AreEqual(false, zero.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", zero.Name);

            Assert.AreEqual("rok", one.Value);
            Assert.AreEqual(false, zero.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", zero.Name);

            Assert.AreEqual("roky", two.Value);
            Assert.AreEqual(false, two.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", two.Name);

            Assert.AreEqual("roky", three.Value);
            Assert.AreEqual(false, three.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", three.Name);

            Assert.AreEqual("roky", four.Value);
            Assert.AreEqual(false, four.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", four.Name);

            Assert.AreEqual("let", five.Value);
            Assert.AreEqual(false, five.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", five.Name);

            Assert.AreEqual("let", six.Value);
            Assert.AreEqual(false, six.ResourceNotFound);
            Assert.AreEqual("klíč-stringu", six.Name);
        }



    }
}