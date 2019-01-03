using System;
using System.Linq;
using Localization.Database.NHibernate.Repository;
using Localization.Database.NHibernate.Tests.Helper;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class StaticTextUoWTest
    {
        private SessionManager m_sessionManager;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionManager = new SessionManager(
                NHibernateConfigurator.GetSessionFactory(nameof(StaticTextUoWTest))
            );
        }

        [TestMethod]
        public void ScopeCrTest()
        {
            var staticTextRepository = new StaticTextRepository(m_sessionManager);

            var cultureRepository = new CultureRepository(m_sessionManager);
            var cultureUoW = new CultureUoW(cultureRepository, m_sessionManager);

            var dictionaryScopeRepository = new DictionaryScopeRepository(m_sessionManager);
            var dictionaryScopeUoW = new DictionaryScopeUoW(dictionaryScopeRepository, m_sessionManager);

            var staticTextUoW = new StaticTextUoW(
                staticTextRepository, cultureRepository, dictionaryScopeRepository, m_sessionManager
            );

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
