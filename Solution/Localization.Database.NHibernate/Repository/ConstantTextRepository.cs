using DryIoc.Facilities.NHibernate;
using Localization.Database.NHibernate.Entity;

namespace Localization.Database.NHibernate.Repository
{
    public class ConstantStaticTextRepository : BaseTextRepository<ConstantStaticTextEntity>
    {
        public ConstantStaticTextRepository(
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
        }
    }
}
