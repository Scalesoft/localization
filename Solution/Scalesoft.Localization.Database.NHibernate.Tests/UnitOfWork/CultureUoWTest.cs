using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Scalesoft.Localization.Database.NHibernate.Tests.Helper;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class CultureUoWTest
    {
        private ISessionFactory m_sessionFactory;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionFactory = NHibernateConfigurator.GetSessionFactory(nameof(CultureUoWTest));
        }

        [TestMethod]
        public void CultureCrTest()
        {
            var cultureUoW = new CultureUoW(m_sessionFactory);

            Assert.IsNull(cultureUoW.GetCultureById(0));
            Assert.IsNull(cultureUoW.GetCultureByName("not-exist"));

            cultureUoW.AddCulture("es");

            var allCultures = cultureUoW.FindAllCultures();
            Assert.AreEqual(1, allCultures.Count);
            Assert.AreEqual("es", allCultures.First().Name);
            Assert.AreEqual("es", cultureUoW.GetCultureById(1).Name);
            Assert.AreEqual("es", cultureUoW.GetCultureByName("es").Name);
        }
    }
}
