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
            LocalizedString lsA = new LocalizedString("name", "value");
            LocalizedString lsB = new LocalizedString("name", "value");

            LocalizedStringComparer localizedStringComparer = new LocalizedStringComparer();
            bool result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsNullTest()
        {
            LocalizedStringComparer localizedStringComparer = new LocalizedStringComparer();
            bool result = localizedStringComparer.Equals(null, null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotEqualsNameTest()
        {
            LocalizedString lsA = new LocalizedString("name", "value");
            LocalizedString lsB = new LocalizedString("jmeno", "value");


            LocalizedStringComparer localizedStringComparer = new LocalizedStringComparer();
            bool result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NotEqualsValueTest()
        {
            LocalizedString lsA = new LocalizedString("name", "value");
            LocalizedString lsB = new LocalizedString("name", "hodnota");


            LocalizedStringComparer localizedStringComparer = new LocalizedStringComparer();
            bool result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotEqualsNameAndValueTest()
        {
            LocalizedString lsA = new LocalizedString("name", "value");
            LocalizedString lsB = new LocalizedString("jmeno", "hodnota");


            LocalizedStringComparer localizedStringComparer = new LocalizedStringComparer();
            bool result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NotEqualsFoundTest()
        {
            LocalizedString lsA = new LocalizedString("name", "value", true);
            LocalizedString lsB = new LocalizedString("name", "value", false);


            LocalizedStringComparer localizedStringComparer = new LocalizedStringComparer();
            bool result = localizedStringComparer.Equals(lsA, lsB);

            Assert.IsTrue(result);
        }


    }
}