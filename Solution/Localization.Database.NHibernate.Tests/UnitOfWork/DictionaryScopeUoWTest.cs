using System.Linq;
using Localization.Database.NHibernate.Tests.Helper;
using Localization.Database.NHibernate.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace Localization.Database.NHibernate.Tests.UnitOfWork
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
        public void ScopeCrTest()
        {
            var dictionaryScopeUoW = new DictionaryScopeUoW(m_sessionFactory);

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
