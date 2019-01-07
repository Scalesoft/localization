using System;
using System.Collections.Generic;
using Localization.Database.NHibernate.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;

namespace Localization.Database.NHibernate.Repository
{
    public class DictionaryScopeRepository : RepositoryBase
    {
        public DictionaryScopeRepository(
            ISession session
        ) : base(session)
        {
        }

        private void FetchCollections(ISession session, ICriterion criterion = null)
        {
            CreateBaseQuery<DictionaryScopeEntity>(session, criterion)
                .Future<DictionaryScopeEntity>();
        }

        public DictionaryScopeEntity GetScopeById(int id)
        {
            try
            {
                var criteria = Restrictions.Where<DictionaryScopeEntity>(x => x.Id == id);

                return GetSingleValue<DictionaryScopeEntity>(FetchCollections, criteria);
            }
            catch (Exception ex)
            {
                throw new DataException("GetScopeById operation failed", ex);
            }
        }

        public DictionaryScopeEntity GetScopeByName(string cultureName)
        {
            try
            {
                var criteria = Restrictions.Where<DictionaryScopeEntity>(x => x.Name == cultureName);

                return GetSingleValue<DictionaryScopeEntity>(FetchCollections, criteria);
            }
            catch (Exception ex)
            {
                throw new DataException("GetScopeByName operation failed", ex);
            }
        }

        public IList<DictionaryScopeEntity> FindAllScopes()
        {
            try
            {
                return GetValuesList<DictionaryScopeEntity>(FetchCollections);
            }
            catch (Exception ex)
            {
                throw new DataException("Find all dictionary scopes operation failed", ex);
            }
        }
    }
}
