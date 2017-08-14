using System;
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
            catch (PluralizedDefaultStringException e)
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
            catch (PluralizedStringIntervalOverlapException e)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}