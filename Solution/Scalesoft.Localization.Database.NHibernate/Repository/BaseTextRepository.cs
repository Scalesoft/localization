using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Exceptions;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Repository
{
    public abstract class BaseTextRepository<T> : RepositoryBase where T : BaseTextEntity
    {
        private const string EmptyArgumentMessage = "Empty argument";

        protected BaseTextRepository(
            ISession session
        ) : base(session)
        {
        }

        protected virtual void FetchCollections(ISession session, ICriterion criterion = null)
        {
            CreateBaseQuery<T>(session, criterion)
                .Fetch(SelectMode.Fetch, x => x.Culture)
                .Fetch(SelectMode.Fetch, x => x.DictionaryScope)
                .Future<T>();
        }

        public T GetStaticTextById(int id)
        {
            try
            {
                var criteria = Restrictions.Where<T>(x => x.Id == id);

                return GetSingleValue<T>(FetchCollections, criteria);
            }
            catch (Exception ex)
            {
                throw new DataException("GetStaticTextById operation failed", ex);
            }
        }

        public T GetByNameAndCultureAndScope(
            string name, string cultureName, string dictionaryScopeName)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(EmptyArgumentMessage, nameof(name));
            if (string.IsNullOrEmpty(cultureName)) throw new ArgumentException(EmptyArgumentMessage, nameof(cultureName));
            if (string.IsNullOrEmpty(dictionaryScopeName)) throw new ArgumentException(EmptyArgumentMessage, nameof(dictionaryScopeName));

            try
            {
                var criteria = Restrictions.Where<T>(
                    x => x.Name == name
                );

                return GetSingleValue<T>(FetchCollections, criteria, query =>
                {
                    DictionaryScopeEntity dictionaryScope = null;
                    CultureEntity culture = null;
                    return query
                        .JoinAlias(x => x.DictionaryScope, () => dictionaryScope)
                        .JoinAlias(x => x.Culture, () => culture)
                        .Where(x => culture.Name == cultureName)
                        .Where(x => dictionaryScope.Name == dictionaryScopeName);
                });
            }
            catch (Exception ex)
            {
                throw new DataException("GetByNameAndCultureAndScope operation failed", ex);
            }
        }

        public T GetByNameAndCultureAndScopeWithHierarchies(
            string name, string cultureName, string dictionaryScopeName
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(EmptyArgumentMessage, nameof(name));
            if (string.IsNullOrEmpty(dictionaryScopeName)) throw new ArgumentException(EmptyArgumentMessage, nameof(dictionaryScopeName));

            try
            {
                
                var session = GetSession();
                
                DictionaryScopeEntity dictionaryScope = null;
                CultureEntity culture = null;
                CultureHierarchyEntity cultureHierarchy = null;
                CultureEntity childCulture = null;

                var query = session.QueryOver<T>()
                    .Where(x => x.Name == name)
                    .Fetch(SelectMode.Fetch, x => x.Culture)
                    .Fetch(SelectMode.Fetch, x => x.DictionaryScope)
                    .JoinAlias(x => x.DictionaryScope, () => dictionaryScope)
                    .JoinAlias(x => x.Culture, () => culture)
                    .Where(x => dictionaryScope.Name == dictionaryScopeName)
                    .JoinAlias(x => culture.ChildCultureHierarchy, () => cultureHierarchy)
                    .JoinAlias(x => cultureHierarchy.Culture, () => childCulture)
                    .Where(x => childCulture.Name == cultureName)
                    .OrderBy(() => cultureHierarchy.LevelProperty).Asc
                    .Take(4).List();

                return query.FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new DataException("GetByNameAndCultureAndScopeWithHierarchies operation failed", ex);
            }
        }

        public IList<T> FindByNameAndScope(
            string name, string dictionaryScopeName
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(EmptyArgumentMessage, nameof(name));
            if (string.IsNullOrEmpty(dictionaryScopeName)) throw new ArgumentException(EmptyArgumentMessage, nameof(dictionaryScopeName));

            try
            {
                var criteria = Restrictions.Where<T>(x => x.Name == name);

                return GetValuesList<T>(FetchCollections, criteria, query =>
                {
                    DictionaryScopeEntity dictionaryScope = null;
                    return query.JoinAlias(x => x.DictionaryScope, () => dictionaryScope)
                        .Where(x => dictionaryScope.Name == dictionaryScopeName);
                });
            }
            catch (Exception ex)
            {
                throw new DataException("GetByNameAndScope operation failed", ex);
            }
        }

        public IList<T> FindAllStaticTexts()
        {
            try
            {
                return GetValuesList<T>(FetchCollections);
            }
            catch (Exception ex)
            {
                throw new DataException("Find all static texts operation failed", ex);
            }
        }

        public IList<T> FindAllByCultureAndScope(string cultureName, string dictionaryScopeName)
        {
            if (string.IsNullOrEmpty(cultureName)) throw new ArgumentException(EmptyArgumentMessage, nameof(cultureName));
            if (string.IsNullOrEmpty(dictionaryScopeName)) throw new ArgumentException(EmptyArgumentMessage, nameof(dictionaryScopeName));

            try
            {
                return GetValuesList<T>(FetchCollections, null, query =>
                {
                    DictionaryScopeEntity dictionaryScope = null;
                    CultureEntity culture = null;
                    return query
                        .JoinAlias(x => x.DictionaryScope, () => dictionaryScope)
                        .JoinAlias(x => x.Culture, () => culture)
                        .Where(x => culture.Name == cultureName)
                        .Where(x => dictionaryScope.Name == dictionaryScopeName);
                });
            }
            catch (Exception ex)
            {
                throw new DataException("FindAllByCultureAndScope operation failed", ex);
            }
        }
    }
}
