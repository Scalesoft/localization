using DryIoc.Facilities.NHibernate;
using Localization.Database.NHibernate.Entity;

namespace Localization.Database.NHibernate.Repository
{
    public class StaticTextRepository : BaseTextRepository<StaticTextEntity>
    {
        public StaticTextRepository(
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
        }
    }
}
