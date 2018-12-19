using System;
using System.Collections.Generic;
using System.Linq;
using DryIoc.Facilities.NHibernate;
using Localization.Database.NHibernate.Dao;
using NHibernate;
using NHibernate.Criterion;

namespace Localization.Database.NHibernate.Repository
{
    public abstract class RepositoryBase : NHibernateDao
    {
        protected RepositoryBase(ISessionManager sessionManager) : base(sessionManager)
        {
        }

        protected IQueryOver<T, T> CreateBaseQuery<T>(ISession session, ICriterion criterion = null) where T : class
        {
            var query = session.QueryOver<T>();

            if (criterion != null)
            {
                query.Where(criterion);
            }

            return query;
        }

        protected T GetSingleValue<T>(Action<ISession, ICriterion> fetchMethod = null, ICriterion criterion = null)
            where T : class
        {
            var session = GetSession();

            var result = CreateBaseQuery<T>(session, criterion).FutureValue<T>();

            fetchMethod?.Invoke(session, criterion);

            return result.Value;
        }

        protected IList<T> GetValuesList<T>(
            Action<ISession, ICriterion> fetchMethod = null, ICriterion criterion = null,
            Func<IQueryOver<T, T>, IQueryOver<T, T>> resultQueryModifier = null
        )
            where T : class
        {
            var session = GetSession();

            var result = CreateBaseQuery<T>(session, criterion);

            fetchMethod?.Invoke(session, criterion);

            return resultQueryModifier != null
                ? resultQueryModifier.Invoke(result).Future<T>().ToList()
                : result.Future<T>().ToList();
        }

        protected IList<T> GetValuesList<T>(
            int start, int count,
            Action<ISession, ICriterion> fetchMethod = null, ICriterion criterion = null,
            Func<IQueryOver<T, T>, IQueryOver<T, T>> resultQueryModifier = null
        ) where T : class
        {
            var session = GetSession();

            var query = CreateBaseQuery<T>(session, criterion);

            if (resultQueryModifier != null)
            {
                query = resultQueryModifier.Invoke(query);
            }

            var result = query.Skip(start)
                .Take(count)
                .Future<T>();

            fetchMethod?.Invoke(session, criterion);

            return result.ToList();
        }
    }
}
