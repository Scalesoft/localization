using System.Collections.Generic;
using DryIoc.Facilities.NHibernate;
using DryIoc.Transactions;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;
using NHibernate;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class CultureUoW : UnitOfWorkBase
    {
        private readonly CultureRepository m_cultureRepository;

        public CultureUoW(
            CultureRepository cultureRepository,
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
            m_cultureRepository = cultureRepository;
        }

        [Transaction]
        public virtual int AddCulture(string name)
        {
            var culture = new CultureEntity
            {
                Name = name
            };

            var result = (int) m_cultureRepository.Create(culture);

            return result;
        }
        
        [Transaction]
        public virtual IList<CultureEntity> FindAllCultures()
        {
            var resultList = m_cultureRepository.FindAllCultures();

            return resultList;
        }
    }
}