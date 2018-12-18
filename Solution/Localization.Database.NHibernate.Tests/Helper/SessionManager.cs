using DryIoc.Facilities.NHibernate;
using NHibernate;

namespace Localization.Database.NHibernate.Tests.Helper
{
    public class SessionManager : ISessionManager
    {
        private readonly ISessionFactory m_sessionFactory;

        public SessionManager(
            ISessionFactory sessionFactory)
        {
            m_sessionFactory = sessionFactory;
        }

        public ISession OpenSession()
        {
            return m_sessionFactory.OpenSession();
        }
    }
}