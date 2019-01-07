using Localization.Database.NHibernate.Entity;
using NHibernate;
using NHibernate.Criterion;

namespace Localization.Database.NHibernate.Repository
{
    public class PluralizedStaticTextRepository : BaseTextRepository<PluralizedStaticTextEntity>
    {
        public PluralizedStaticTextRepository(
            ISession session
        ) : base(session)
        {
        }

        protected override void FetchCollections(ISession session, ICriterion criterion = null)
        {
            CreateBaseQuery<PluralizedStaticTextEntity>(session, criterion)
                .Fetch(SelectMode.Fetch, x => x.Culture)
                .Fetch(SelectMode.Fetch, x => x.DictionaryScope)
                .Fetch(SelectMode.Fetch, x => x.IntervalTexts)
                .Future<PluralizedStaticTextEntity>();
        }
    }
}
