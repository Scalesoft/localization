using NHibernate;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Repository
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
