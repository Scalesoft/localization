using System.Linq;
using Localization.Database.NHibernate.Repository;
using Localization.Database.NHibernate.Tests.Helper;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.Database.NHibernate.Tests.UnitOfWork
{
    [TestClass]
    public class DictionaryScopeUoWTest
    {
        private SessionManager m_sessionManager;

        [TestInitialize]
        public void InitTest()
        {
            m_sessionManager = new SessionManager(
                NHibernateConfigurator.GetSessionFactory(nameof(DictionaryScopeUoWTest))
            );
        }

        [TestMethod]
        public void ScopeCrTest()
        {
            var dictionaryScopeRepository = new DictionaryScopeRepository(m_sessionManager);
            var dictionaryScopeUoW = new DictionaryScopeUoW(dictionaryScopeRepository, m_sessionManager);

            Assert.AreEqual(null, dictionaryScopeUoW.GetScopeById(0));
            Assert.AreEqual(null, dictionaryScopeUoW.GetScopeByName("not-exist"));

            dictionaryScopeUoW.AddScope("global");

            var allScopes = dictionaryScopeUoW.FindAllScopes();
            Assert.AreEqual(1, allScopes.Count);
            Assert.AreEqual("global", allScopes.First().Name);
            Assert.AreEqual("global", dictionaryScopeUoW.GetScopeById(1).Name);
            Assert.AreEqual("global", dictionaryScopeUoW.GetScopeByName("global").Name);
        }
    }
}
