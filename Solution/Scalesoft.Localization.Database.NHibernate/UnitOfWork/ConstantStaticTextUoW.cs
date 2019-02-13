using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NHibernate;
using Scalesoft.Localization.Database.Abstractions.Entity;
using Scalesoft.Localization.Database.NHibernate.Entity;
using Scalesoft.Localization.Database.NHibernate.Repository;

namespace Scalesoft.Localization.Database.NHibernate.UnitOfWork
{
    public class ConstantStaticTextUoW : BaseTextUoW
    {
        public ConstantStaticTextUoW(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public virtual IList<ConstantStaticTextEntity> FindAllByCultureAndScope(string cultureName, string dictionaryScopeName)
        {
            using (var session = GetSession())
            {
                var dictionaryScopeRepository = new ConstantStaticTextRepository(session);

                var resultList = dictionaryScopeRepository.FindAllByCultureAndScope(cultureName, dictionaryScopeName);

                return resultList;
            }
        }
    }
}