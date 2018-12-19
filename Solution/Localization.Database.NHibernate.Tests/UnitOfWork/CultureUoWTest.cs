using System.Linq;
using Localization.Database.NHibernate.Repository;
using Localization.Database.NHibernate.Tests.Helper;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Cfg;

namespace Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class CultureUoWTest
    {
        private SessionManager m_sessionManager;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionManager = new SessionManager(
                NHibernateConfigurator.GetSessionFactory(nameof(CultureUoWTest))
            );
        }

        [TestMethod]
        public void CultureCrTest()
        {
            var cultureRepository = new CultureRepository(m_sessionManager);
            var cultureUoW = new CultureUoW(cultureRepository, m_sessionManager);

            Assert.AreEqual(null, cultureUoW.GetCultureById(0));
            Assert.AreEqual(null, cultureUoW.GetCultureByName("not-exist"));

            cultureUoW.AddCulture("es");

            var allCultures = cultureUoW.FindAllCultures();
            Assert.AreEqual(1, allCultures.Count);
            Assert.AreEqual("es", allCultures.First().Name);
            Assert.AreEqual("es", cultureUoW.GetCultureById(1).Name);
            Assert.AreEqual("es", cultureUoW.GetCultureByName("es").Name);
        }
    }
}
