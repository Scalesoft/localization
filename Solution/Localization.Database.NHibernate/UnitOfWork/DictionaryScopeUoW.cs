using System.Collections.Generic;
using DryIoc.Facilities.NHibernate;
using DryIoc.Transactions;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class DictionaryScopeUoW : UnitOfWorkBase
    {
        private readonly DictionaryScopeRepository m_dictionaryScopeRepository;

        public DictionaryScopeUoW(
            DictionaryScopeRepository dictionaryScopeRepository,
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
            m_dictionaryScopeRepository = dictionaryScopeRepository;
        }

        [Transaction]
        public virtual int AddScope(string name)
        {
            var culture = new DictionaryScopeEntity
            {
                Name = name
            };

            var result = (int) m_dictionaryScopeRepository.Create(culture);

            return result;
        }

        [Transaction]
        public virtual DictionaryScopeEntity GetScopeById(int id)
        {
            var resultList = m_dictionaryScopeRepository.GetScopeById(id);

            return resultList;
        }

        [Transaction]
        public virtual DictionaryScopeEntity GetScopeByName(string cultureName)
        {
            var resultList = m_dictionaryScopeRepository.GetScopeByName(cultureName);

            return resultList;
        }

        [Transaction]
        public virtual IList<DictionaryScopeEntity> FindAllScopes()
        {
            var resultList = m_dictionaryScopeRepository.FindAllScopes();

            return resultList;
        }
    }
}