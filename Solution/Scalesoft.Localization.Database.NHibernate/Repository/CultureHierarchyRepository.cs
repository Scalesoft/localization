using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Repository
{
    public class CultureHierarchyRepository : RepositoryBase
    {
        public CultureHierarchyRepository(
            ISession session
        ) : base(session)
        {
        }

        private void FetchCollections(ISession session, ICriterion criterion = null)
        {
            CreateBaseQuery<CultureHierarchyEntity>(session, criterion)
                .Fetch(SelectMode.Fetch, x => x.Culture)
                .Fetch(SelectMode.Fetch, x => x.ParentCulture)
                .Future<CultureHierarchyEntity>();
        }

        private IQueryOver<T, T> ResultQueryModifier<T>(IQueryOver<T, T> result)
            where T : CultureHierarchyEntity
        {
            return result.OrderBy(x => x.LevelProperty).Asc;
        }

        public CultureHierarchyEntity GetCultureHierarchyById(int id)
        {
            try
            {
                var criteria = Restrictions.Where<CultureHierarchyEntity>(x => x.Id == id);

                return GetSingleValue<CultureHierarchyEntity>(FetchCollections, criteria);
            }
            catch (Exception ex)
            {
                throw new DataException("GetCultureHierarchyById operation failed", ex);
            }
        }

        public IList<CultureHierarchyEntity> FindCultureHierarchyByCulture(CultureEntity culture)
        {
            try
            {
                var criteria = Restrictions.Where<CultureHierarchyEntity>(x => Equals(x.Culture, culture));

                return GetValuesList<CultureHierarchyEntity>(FetchCollections, criteria, ResultQueryModifier);
            }
            catch (Exception ex)
            {
                throw new DataException("FindCultureHierarchyByCulture operation failed", ex);
            }
        }

        public IList<CultureHierarchyEntity> FindAllCultureHierarchies()
        {
            try
            {
                return GetValuesList<CultureHierarchyEntity>(FetchCollections, null, ResultQueryModifier);
            }
            catch (Exception ex)
            {
                throw new DataException("FindAllCultureHierarchies operation failed", ex);
            }
        }
    }
}
