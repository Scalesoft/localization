using System;
using System.Collections.Generic;
using Localization.Database.NHibernate.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;

namespace Localization.Database.NHibernate.Repository
{
    public class CultureRepository : RepositoryBase
    {
        public CultureRepository(
            ISession session
        ) : base(session)
        {
        }

        private void FetchCollections(ISession session, ICriterion criterion = null)
        {
            CreateBaseQuery<CultureEntity>(session, criterion)
                .Future<CultureEntity>();
        }

        public CultureEntity GetCultureById(int id)
        {
            try
            {
                var criteria = Restrictions.Where<CultureEntity>(x => x.Id == id);

                return GetSingleValue<CultureEntity>(FetchCollections, criteria);
            }
            catch (Exception ex)
            {
                throw new DataException("GetCultureById operation failed", ex);
            }
        }

        public CultureEntity GetCultureByName(string cultureName)
        {
            try
            {
                var criteria = Restrictions.Where<CultureEntity>(x => x.Name == cultureName);

                return GetSingleValue<CultureEntity>(FetchCollections, criteria);
            }
            catch (Exception ex)
            {
                throw new DataException("GetByName operation failed", ex);
            }
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