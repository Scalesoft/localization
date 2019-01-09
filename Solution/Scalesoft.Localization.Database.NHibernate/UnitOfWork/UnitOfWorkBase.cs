using NHibernate;

namespace Scalesoft.Localization.Database.NHibernate.UnitOfWork
{
    public abstract class UnitOfWorkBase
    {
        private readonly ISessionFactory m_sessionFactory;

        protected UnitOfWorkBase(ISessionFactory sessionFactory)
        {
            m_sessionFactory = sessionFactory;
        }

        protected ISession GetSession()
        {
            return m_sessionFactory.OpenSession();
        }
    }
}