using System;
using System.Collections.Generic;
using DryIoc.Facilities.NHibernate;
using Localization.Database.NHibernate.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;

namespace Localization.Database.NHibernate.Repository
{
    public class CultureRepository : RepositoryBase
    {
        public CultureRepository(
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
        }

        private void FetchCollections(ISession session, ICriterion criterion = null)
        {
            CreateBaseQuery<CultureEntity>(session, criterion)
                .Future<CultureEntity>();
        }

        public IList<CultureEntity> FindAllCultures()
        {
            try
            {
                return GetValuesList<CultureEntity>(FetchCollections);
            }
            catch (Exception ex)
            {
                throw new DataException("Find all cultures operation failed", ex);
            }
        }
    }
}