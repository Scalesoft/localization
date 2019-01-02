using System;
using Localization.CoreLibrary.Dictionary.Factory;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Pluralization
{
    [TestClass]
    public class PluralizedStringTest
    {
        [TestMethod]
        public void BuildSimplePluralizedStringTest()
        {
            var defaultLocalizedString = new LocalizedString("let", "let");
            var psA = new PluralizedString(defaultLocalizedString);

            Assert.AreEqual(defaultLocalizedString, psA.GetPluralizedLocalizedString(0));
        }

        [TestMethod]
        public void ConstructorDefaultStringNullTest()
        {
            var exceptionThrown = false;
            try
            {
                var psA = new PluralizedString(null);
            }
            catch (ArgumentNullException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void IntervalCompletePluralizedStringTest()
        {
            var defaultLocalizedString = new LocalizedString("let", "let");
            var psA = new PluralizedString(defaultLocalizedString);
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
            var exceptionThrown = false;

            var defaultLocalizedString = new LocalizedString("let", "let");
            var psA = new PluralizedString(defaultLocalizedString);
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
            var dictionaryFactory = new JsonDictionaryFactory();
            var localizationDictionary = dictionaryFactory.CreateDictionary(@"Localization\slovniky\slovniky.cs.json");
            var localizedPluralizedStrings = localizationDictionary.ListPlurals();
            Assert.AreEqual(true, localizedPluralizedStrings.ContainsKey("klíč-stringu"));

            var pluralizedString = localizedPluralizedStrings["klíč-stringu"];

            var min = pluralizedString.GetPluralizedLocalizedString(int.MinValue);
            var max = pluralizedString.GetPluralizedLocalizedString(int.MaxValue);
            var zero = pluralizedString.GetPluralizedLocalizedString(0);
            var one = pluralizedString.GetPluralizedLocalizedString(1);
            var two = pluralizedString.GetPluralizedLocalizedString(2);
            var three = pluralizedString.GetPluralizedLocalizedString(3);
            var four = pluralizedString.GetPluralizedLocalizedString(4);
            var five = pluralizedString.GetPluralizedLocalizedString(5);
            var six = pluralizedString.GetPluralizedLocalizedString(6);

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


        [TestMethod]
        public void PluralizedDictionaryLoadTest()
        {
            var dictionaryFactory = new JsonDictionaryFactory();
            var localizationDictionary = dictionaryFactory.CreateDictionary(@"Localization\slovniky\slovniky.cs.json");
            var localizedPluralizedStrings = localizationDictionary.ListPlurals();
            Assert.AreEqual(1, localizedPluralizedStrings.Count);

            Assert.AreEqual(true, localizedPluralizedStrings.ContainsKey("klíč-stringu"));

            var pluralizedString = localizedPluralizedStrings["klíč-stringu"];

            var min = pluralizedString.GetPluralizedLocalizedString(int.MinValue);
            var max = pluralizedString.GetPluralizedLocalizedString(int.MaxValue);
            var zero = pluralizedString.GetPluralizedLocalizedString(0);
            var one = pluralizedString.GetPluralizedLocalizedString(1);
            var two = pluralizedString.GetPluralizedLocalizedString(2);
            var three = pluralizedString.GetPluralizedLocalizedString(3);
            var four = pluralizedString.GetPluralizedLocalizedString(4);
            var five = pluralizedString.GetPluralizedLocalizedString(5);
            var six = pluralizedString.GetPluralizedLocalizedString(6);

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
