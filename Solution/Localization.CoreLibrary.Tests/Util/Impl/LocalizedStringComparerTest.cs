using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Util.Impl
{
    [TestClass]
    public class LocalizedStringComparerTest
    {
        [TestMethod]
        public void EqualsTest()
        {
            var lsA = new LocalizedString("name", "value");
            var lsB = new LocalizedString("name", "value");

            var localizedStringComparer = new LocalizedStringComparer();
            var result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsNullTest()
        {
            var localizedStringComparer = new LocalizedStringComparer();
            var result = localizedStringComparer.Equals(null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotEqualsNameTest()
        {
            var lsA = new LocalizedString("name", "value");
            var lsB = new LocalizedString("jmeno", "value");


            var localizedStringComparer = new LocalizedStringComparer();
            var result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NotEqualsValueTest()
        {
            var lsA = new LocalizedString("name", "value");
            var lsB = new LocalizedString("name", "hodnota");


            var localizedStringComparer = new LocalizedStringComparer();
            var result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotEqualsNameAndValueTest()
        {
            var lsA = new LocalizedString("name", "value");
            var lsB = new LocalizedString("jmeno", "hodnota");


            var localizedStringComparer = new LocalizedStringComparer();
            var result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NotEqualsFoundTest()
        {
            var lsA = new LocalizedString("name", "value", true);
            var lsB = new LocalizedString("name", "value", false);


            var localizedStringComparer = new LocalizedStringComparer();
            var result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsTrue(result);
        }
    }
}