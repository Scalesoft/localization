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
        private Configuration m_nHibernateConfigurator;
        private SessionManager m_sessionManager;

        [TestInitialize]
        public void InitTest()
        {
            m_nHibernateConfigurator = NHibernateConfigurator.GetNHibernateConfigurator();
            m_sessionManager = new SessionManager(NHibernateConfigurator.GetSessionFactory(m_nHibernateConfigurator));
        }

        [TestMethod]
        public void TranslateConcurrentlyTest()
        {
            var cultureRepository = new CultureRepository(m_sessionManager);
            var cultureUoW = new CultureUoW(cultureRepository, m_sessionManager);

            cultureUoW.AddCulture("es");

            Assert.AreEqual(1, cultureUoW.FindAllCultures().Count);
        }
    }
}