using Localization.Database.NHibernate.Entity;
using NHibernate;

namespace Localization.Database.NHibernate.Repository
{
    public class StaticTextRepository : BaseTextRepository<StaticTextEntity>
    {
        public StaticTextRepository(
            ISession session
        ) : base(session)
        {
        }
    }
}
