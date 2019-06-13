using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Pluralization;

namespace Scalesoft.Localization.Core.Tests.Pluralization
{
    [TestClass]
    public class PluralizationIntervalTest
    {
        [TestMethod]
        public void IsOverlapingEqualsTest()
        {
            var pA = new PluralizationInterval(0, 1);
            const int testNumber = 1;

            var result = pA.IsInInterval(testNumber);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingEqualsOneTest()
        {
            var pA = new PluralizationInterval(0, 0);
            const int testNumber = 0;

            var result = pA.IsInInterval(testNumber);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOvelapingNotEqualsTest()
        {
            var pA = new PluralizationInterval(0, 0);
            const int testNumber = 2;

            var result = pA.IsInInterval(testNumber);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsOvelapingTestConstructorException()
        {
            Assert.ThrowsException<ArgumentException>(() => new PluralizationInterval(2, 0));
        }
    }
}