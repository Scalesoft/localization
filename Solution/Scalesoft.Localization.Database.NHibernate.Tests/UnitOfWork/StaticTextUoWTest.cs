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
        public void StaticTextCreateReadTest()
        {
            var cultureUoW = new CultureUoW(m_sessionFactory);
            var dictionaryScopeUoW = new DictionaryScopeUoW(m_sessionFactory);
            var staticTextUoW = new StaticTextUoW(m_sessionFactory);

            cultureUoW.AddCulture("cs");
            dictionaryScopeUoW.AddScope("dictionaryScope");

            Assert.IsNull(staticTextUoW.GetStaticTextById(0));
            Assert.IsNull(staticTextUoW.GetByNameAndCultureAndScope("not-exist", "not-exist", "not-exist"));

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
            Assert.IsNull(nullStaticText1);
            Assert.IsNull(nullStaticText2);
            Assert.IsNull(nullStaticText3);
        }

        [TestMethod]
        public void StaticTextCreateUpdateTest()
        {
            var cultureUoW = new CultureUoW(m_sessionFactory);
            var dictionaryScopeUoW = new DictionaryScopeUoW(m_sessionFactory);
            var staticTextUoW = new StaticTextUoW(m_sessionFactory);

            cultureUoW.AddCulture("cs");
            dictionaryScopeUoW.AddScope("dictionaryScope");

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

            var staticText = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "cs",
                "dictionaryScope"
            );
            Assert.AreEqual("name", staticText.Name);
            Assert.AreEqual("text", staticText.Text);

            staticTextUoW.UpdateStaticText(
                "name",
                "cs",
                "dictionaryScope",
                0,
                "modifiedText",
                "modificationUser",
                time
            );

            var staticTextReFetched = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "cs",
                "dictionaryScope"
            );

            Assert.AreEqual("modifiedText", staticTextReFetched.Text);
        }

        [TestMethod]
        public void StaticTextCreateDeleteTest()
        {
            var cultureUoW = new CultureUoW(m_sessionFactory);
            var dictionaryScopeUoW = new DictionaryScopeUoW(m_sessionFactory);
            var staticTextUoW = new StaticTextUoW(m_sessionFactory);

            cultureUoW.AddCulture("cs");
            dictionaryScopeUoW.AddScope("dictionaryScope");

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

            var staticText = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "cs",
                "dictionaryScope"
            );
            Assert.AreEqual("name", staticText.Name);
            Assert.AreEqual("text", staticText.Text);

            staticTextUoW.Delete(
                "name",
                "cs",
                "dictionaryScope"
            );

            var staticTextReFetched = staticTextUoW.GetByNameAndCultureAndScope(
                "name",
                "cs",
                "dictionaryScope"
            );

            Assert.IsNull(staticTextReFetched);
        }
    }
}