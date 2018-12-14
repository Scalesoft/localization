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
            var pA = new PluralizationInterval(0, 1);
            var pB = new PluralizationInterval(0, 1);

            var result = pA.IsOverlaping(pB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingEqualsOneTest()
        {
            var pA = new PluralizationInterval(0, 0);
            var pB = new PluralizationInterval(0, 0);

            var result = pA.IsOverlaping(pB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingNotEqualsTest()
        {
            var pA = new PluralizationInterval(0, 0);
            var pB = new PluralizationInterval(1, 2);

            var result = pA.IsOverlaping(pB);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsOvelapingOverlapsTest()
        {
            var pA = new PluralizationInterval(int.MinValue, int.MaxValue);
            var pB = new PluralizationInterval(-2, 2);

            var result = pA.IsOverlaping(pB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingTestConstructorException()
        {
            var exceptionThrown = false;
            try
            {
                var pA = new PluralizationInterval(2, 0);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }
    }
}