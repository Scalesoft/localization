using System;
using Localization.CoreLibrary.Pluralization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Pluralization
{
    [TestClass]
    public class PluralizationIntervalTest
    {
        [TestMethod]
        public void IsOverlapingEqualsTest()
        {
            PluralizationInterval pA = new PluralizationInterval(0, 1);
            PluralizationInterval pB = new PluralizationInterval(0, 1);

            bool result = pA.IsOverlaping(pB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingEqualsOneTest()
        {
            PluralizationInterval pA = new PluralizationInterval(0, 0);
            PluralizationInterval pB = new PluralizationInterval(0, 0);

            bool result = pA.IsOverlaping(pB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingNotEqualsTest()
        {
            PluralizationInterval pA = new PluralizationInterval(0, 0);
            PluralizationInterval pB = new PluralizationInterval(1, 2);

            bool result = pA.IsOverlaping(pB);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsOvelapingOverlapsTest()
        {
            PluralizationInterval pA = new PluralizationInterval(int.MinValue, int.MaxValue);
            PluralizationInterval pB = new PluralizationInterval(-2, 2);

            bool result = pA.IsOverlaping(pB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingTestConstructorException()
        {
            bool exceptionThrown = false;
            try
            {
                PluralizationInterval pA = new PluralizationInterval(2, 0);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}