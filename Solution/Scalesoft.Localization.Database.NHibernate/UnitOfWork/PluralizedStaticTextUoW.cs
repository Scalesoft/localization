using System.Collections.Generic;
using NHibernate;
using Scalesoft.Localization.Database.Abstractions.Entity;
using Scalesoft.Localization.Database.NHibernate.Entity;
using Scalesoft.Localization.Database.NHibernate.Repository;

namespace Scalesoft.Localization.Database.NHibernate.UnitOfWork
{
    public class PluralizedStaticTextUoW : BaseTextUoW
    {
        public PluralizedStaticTextUoW(
            ISessionFactory sessionFactory
        ) : base(sessionFactory)
        {
        }

        public virtual IList<PluralizedStaticTextEntity> FindAllByCultureAndScope(string cultureName, string dictionaryScopeName)
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new PluralizedStaticTextRepository(session);

                var result = staticTextRepository.FindAllByCultureAndScope(cultureName, dictionaryScopeName);

                return result;
            }
        }
    }
}