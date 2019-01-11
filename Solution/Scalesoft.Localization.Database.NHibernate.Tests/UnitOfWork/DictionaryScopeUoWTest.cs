using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Scalesoft.Localization.Database.NHibernate.Tests.Helper;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class DictionaryScopeUoWTest
    {
        private ISessionFactory m_sessionFactory;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionFactory = NHibernateConfigurator.GetSessionFactory(nameof(DictionaryScopeUoWTest));
        }

        [TestMethod]
        public void DictionaryCrTest()
        {
            var dictionaryScopeUoW = new DictionaryScopeUoW(m_sessionFactory);

            Assert.IsNull(dictionaryScopeUoW.GetScopeById(0));
            Assert.IsNull(dictionaryScopeUoW.GetScopeByName("not-exist"));

            dictionaryScopeUoW.AddScope("global");

            var allScopes = dictionaryScopeUoW.FindAllScopes();
            Assert.AreEqual(1, allScopes.Count);
            Assert.AreEqual("global", allScopes.First().Name);
            Assert.AreEqual("global", dictionaryScopeUoW.GetScopeById(1).Name);
            Assert.AreEqual("global", dictionaryScopeUoW.GetScopeByName("global").Name);
        }
    }
}
