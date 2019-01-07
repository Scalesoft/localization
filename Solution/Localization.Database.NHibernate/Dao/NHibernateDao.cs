using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate;

namespace Localization.Database.NHibernate.Dao
{
    public class NHibernateDao
    {
        private readonly ISession m_session;
        public const string WildcardAny = "%";
        public const string WildcardSingle = "_";

        protected NHibernateDao(ISession session)
        {
            m_session = session;
        }

        protected ISession GetSession()
        {
            return m_session;
        }

        public static string EscapeQuery(string query)
        {
            return query?.Replace("[", "[[]");
        }

        public virtual object FindById(Type type, object id)
        {
            try
            {
                return GetSession().Get(type, id);
            }
            catch (Exception ex)
            {
                throw new DataException($"Get by id operation failed for type:{type.Name}", ex);
            }
        }

        public virtual T FindById<T>(object id)
        {
            return (T) FindById(typeof(T), id);
        }

        public virtual object Load(Type type, object id)
        {
            try
            {
                return GetSession().Load(type, id);
            }
            catch (ObjectNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataException($"Load by id operation failed for type:{type.Name}", ex);
            }
        }

        public virtual T Load<T>(object id)
        {
            return (T) Load(typeof(T), id);
        }

        public virtual object Create(object instance)
        {
            try
            {
                return GetSession().Save(instance);
            }
            catch (Exception ex)
            {
                throw new DataException($"Create operation failed for type:{instance.GetType().Name}", ex);
            }
        }

        public virtual IList<object> CreateAll(IEnumerable data)
        {
            var result = new List<object>();
            foreach (var instance in data)
            {
                var id = Create(instance);
                result.Add(id);
            }

            return result;
        }

        public virtual void Delete(object instance)
        {
            try
            {
                GetSession().Delete(instance);
            }
            catch (Exception ex)
            {
                throw new DataException($"Delete operation failed for type:{instance.GetType().Name}", ex);
            }
        }

        public virtual void Update(object instance)
        {
            try
            {
                GetSession().Update(instance);
            }
            catch (Exception ex)
            {
                throw new DataException($"Update operation failed for type:{instance.GetType().Name}",
                    ex);
            }
        }

        public virtual void DeleteAll(Type type)
        {
            try
            {
                GetSession().Delete($"from {type.Name}");
            }
            catch (Exception ex)
            {
                throw new DataException($"Delete operation failed for type:{type.Name}", ex);
            }
        }

        public virtual void DeleteAll(IEnumerable data)
        {
            foreach (var o in data)
            {
                Delete(o);
            }
        }

        public virtual void Save(object instance)
        {
            try
            {
                GetSession().SaveOrUpdate(instance);
            }
            catch (Exception ex)
            {
                throw new DataException($"Save or Update operation failed for type:{instance.GetType().Name}", ex);
            }
        }

        public virtual void SaveAll(IEnumerable data)
        {
            foreach (var o in data)
            {
                Save(o);
            }
        }
    }
}