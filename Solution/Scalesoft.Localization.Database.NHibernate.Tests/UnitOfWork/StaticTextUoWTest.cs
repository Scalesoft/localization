using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Scalesoft.Localization.Database.NHibernate.Tests.Helper;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class StaticTextUoWTest
    {
        private ISessionFactory m_sessionFactory;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionFactory = NHibernateConfigurator.GetSessionFactory(nameof(StaticTextUoWTest));
        }

        [TestMethod]
        public void ScopeCrTest()
        {
            var cultureUoW = new CultureUoW(m_sessionFactory);
            var dictionaryScopeUoW = new DictionaryScopeUoW(m_sessionFactory);
            var staticTextUoW = new StaticTextUoW(m_sessionFactory);

            cultureUoW.AddCulture("cs");
            dictionaryScopeUoW.AddScope("dictionaryScope");

            Assert.AreEqual(null, staticTextUoW.GetStaticTextById(0));
            Assert.AreEqual(null, staticTextUoW.GetByNameAndCultureAndScope("not-exist", "not-exist", "not-exist"));

            var time = DateTime.UtcNow;
            staticTextUoW.AddStaticText(
                "name",
                0,
                "text",
                "cs",
                "dictionaryScope",
                "modificationUser",
                time
            );

            var allStaticTexts = staticTextUoW.FindAllStaticTexts();
            Assert.AreEqual(1, allStaticTexts.Count);
            Assert.AreEqual("name", allStaticTexts.First().Name);
            Assert.AreEqual("name", staticTextUoW.GetStaticTextById(1).Name);
            var staticText = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "cs",
                "dictionaryScope"
            );
            Assert.AreEqual("name", staticText.Name);
            Assert.AreEqual("text", staticText.Text);

            var nullStaticText1 = staticTextUoW.GetByNameAndCultureAndScope(
                "not-exist",
                "cs",
                "dictionaryScope"
            );
            var nullStaticText2 = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "en",
                "dictionaryScope"
            );
            var nullStaticText3 = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "cs",
                "not-exist"
            );
            Assert.AreEqual(null, nullStaticText1);
            Assert.AreEqual(null, nullStaticText2);
            Assert.AreEqual(null, nullStaticText3);
        }
    }
}
