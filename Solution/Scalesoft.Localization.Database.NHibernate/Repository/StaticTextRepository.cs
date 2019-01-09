using NHibernate;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Repository
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
