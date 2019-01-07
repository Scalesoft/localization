using Localization.Database.NHibernate.Entity;
using NHibernate;

namespace Localization.Database.NHibernate.Repository
{
    public class ConstantStaticTextRepository : BaseTextRepository<ConstantStaticTextEntity>
    {
        public ConstantStaticTextRepository(
            ISession session
        ) : base(session)
        {
        }
    }
}
