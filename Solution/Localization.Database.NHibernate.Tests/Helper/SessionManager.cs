using DryIoc.Facilities.NHibernate;
using NHibernate;

namespace Localization.Database.NHibernate.Tests.Helper
{
    public class SessionManager : ISessionManager
    {
        private readonly ISessionFactory m_session;

        public SessionManager(
            ISessionFactory session
        )
        {
            m_session = session;
        }

        public ISession OpenSession()
        {
            return m_session.OpenSession();
        }
    }
}
