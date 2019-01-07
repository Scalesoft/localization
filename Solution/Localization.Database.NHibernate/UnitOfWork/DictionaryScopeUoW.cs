using System.Collections.Generic;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;
using NHibernate;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class DictionaryScopeUoW : UnitOfWorkBase
    {
        public DictionaryScopeUoW(
            ISessionFactory sessionFactory
        ) : base(sessionFactory)
        {
        }

        public virtual int AddScope(string name)
        {
            using (var session = GetSession())
            {
                var dictionaryScopeRepository = new DictionaryScopeRepository(session);

                var culture = new DictionaryScopeEntity
                {
                    Name = name
                };

                var result = (int) dictionaryScopeRepository.Create(culture);

                return result;
            }
        }

        public virtual DictionaryScopeEntity GetScopeById(int id)
        {
            using (var session = GetSession())
            {
                var dictionaryScopeRepository = new DictionaryScopeRepository(session);

                var resultList = dictionaryScopeRepository.GetScopeById(id);

                return resultList;
            }
        }

        public virtual DictionaryScopeEntity GetScopeByName(string cultureName)
        {
            using (var session = GetSession())
            {
                var dictionaryScopeRepository = new DictionaryScopeRepository(session);

                var resultList = dictionaryScopeRepository.GetScopeByName(cultureName);

                return resultList;
            }
        }

        public virtual IList<DictionaryScopeEntity> FindAllScopes()
        {
            using (var session = GetSession())
            {
                var dictionaryScopeRepository = new DictionaryScopeRepository(session);

                var resultList = dictionaryScopeRepository.FindAllScopes();

                return resultList;
            }
        }
    }
}