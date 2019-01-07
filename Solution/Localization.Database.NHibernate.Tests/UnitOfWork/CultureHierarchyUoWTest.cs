using Localization.Database.NHibernate.Tests.Helper;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class CultureHierarchyUoWTest
    {
        private ISessionFactory m_sessionFactory;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionFactory = NHibernateConfigurator.GetSessionFactory(nameof(CultureHierarchyUoWTest));
        }

        [TestMethod]
        public void CultureHierarchyCrTest()
        {
            var cultureUoW = new CultureUoW(m_sessionFactory);
            var cultureHierarchyUoW = new CultureHierarchyUoW(m_sessionFactory);


            var cultureCs = cultureUoW.GetCultureById(cultureUoW.AddCulture("cs"));
            var cultureCsCz = cultureUoW.GetCultureById(cultureUoW.AddCulture("cs-CZ"));
            var cultureEn = cultureUoW.GetCultureById(cultureUoW.AddCulture("en"));
            var cultureEnUs = cultureUoW.GetCultureById(cultureUoW.AddCulture("en-US"));

            cultureHierarchyUoW.AddCultureHierarchy(cultureEnUs, cultureCs, 2);

            cultureHierarchyUoW.AddCultureHierarchy(cultureCs, cultureCs, 0);
            cultureHierarchyUoW.AddCultureHierarchy(cultureCsCz, cultureCsCz, 0);
            cultureHierarchyUoW.AddCultureHierarchy(cultureEn, cultureEn, 0);
            cultureHierarchyUoW.AddCultureHierarchy(cultureEnUs, cultureEnUs, 0);

            cultureHierarchyUoW.AddCultureHierarchy(cultureCsCz, cultureCs, 1);
            cultureHierarchyUoW.AddCultureHierarchy(cultureEnUs, cultureEn, 1);
            cultureHierarchyUoW.AddCultureHierarchy(cultureEn, cultureCs, 1);

            var cultureCsHierarchy = cultureHierarchyUoW.FindCultureHierarchyByCulture(cultureCs);
            var cultureEnHierarchy = cultureHierarchyUoW.FindCultureHierarchyByCulture(cultureEn);
            var cultureCsCzHierarchy = cultureHierarchyUoW.FindCultureHierarchyByCulture(cultureCsCz);
            var cultureEnUsHierarchy = cultureHierarchyUoW.FindCultureHierarchyByCulture(cultureEnUs);

            Assert.AreEqual(1, cultureCsHierarchy.Count);
            Assert.AreEqual(2, cultureEnHierarchy.Count);
            Assert.AreEqual(2, cultureCsCzHierarchy.Count);
            Assert.AreEqual(3, cultureEnUsHierarchy.Count);

            Assert.AreEqual(cultureCs.Name, cultureCsHierarchy[0].ParentCulture.Name);
            Assert.AreEqual(0, cultureCsHierarchy[0].LevelProperty);

            Assert.AreEqual(cultureEn.Name, cultureEnHierarchy[0].ParentCulture.Name);
            Assert.AreEqual(0, cultureEnHierarchy[0].LevelProperty);
            Assert.AreEqual(cultureCs.Name, cultureEnHierarchy[1].ParentCulture.Name);
            Assert.AreEqual(1, cultureEnHierarchy[1].LevelProperty);

            Assert.AreEqual(cultureCsCz.Name, cultureCsCzHierarchy[0].ParentCulture.Name);
            Assert.AreEqual(0, cultureCsCzHierarchy[0].LevelProperty);
            Assert.AreEqual(cultureCs.Name, cultureCsCzHierarchy[1].ParentCulture.Name);
            Assert.AreEqual(1, cultureCsCzHierarchy[1].LevelProperty);

            Assert.AreEqual(cultureEnUs.Name, cultureEnUsHierarchy[0].ParentCulture.Name);
            Assert.AreEqual(0, cultureEnUsHierarchy[0].LevelProperty);
            Assert.AreEqual(cultureEn.Name, cultureEnUsHierarchy[1].ParentCulture.Name);
            Assert.AreEqual(1, cultureEnUsHierarchy[1].LevelProperty);
            Assert.AreEqual(cultureCs.Name, cultureEnUsHierarchy[2].ParentCulture.Name);
            Assert.AreEqual(2, cultureEnUsHierarchy[2].LevelProperty);
        }
    }
}
