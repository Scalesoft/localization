using System.Collections.Generic;
using DryIoc.Facilities.NHibernate;
using DryIoc.Transactions;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class CultureHierarchyUoW : UnitOfWorkBase
    {
        private readonly CultureHierarchyRepository m_cultureHierarchyRepository;

        public CultureHierarchyUoW(
            CultureHierarchyRepository cultureHierarchyRepository,
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
            m_cultureHierarchyRepository = cultureHierarchyRepository;
        }

        [Transaction]
        public virtual int AddCultureHierarchy(
            CultureEntity culture,
            CultureEntity parentCulture,
            byte levelProperty
        )
        {
            var cultureHierarchy = new CultureHierarchyEntity
            {
                Culture = culture,
                ParentCulture = parentCulture,
                LevelProperty = levelProperty,
            };

            var result = (int) m_cultureHierarchyRepository.Create(cultureHierarchy);

            return result;
        }

        [Transaction]
        public virtual CultureHierarchyEntity GetCultureHierarchyById(int id)
        {
            var resultList = m_cultureHierarchyRepository.GetCultureHierarchyById(id);

            return resultList;
        }

        [Transaction]
        public virtual IList<CultureHierarchyEntity> FindCultureHierarchyByCulture(CultureEntity culture)
        {
            var resultList = m_cultureHierarchyRepository.FindCultureHierarchyByCulture(culture);

            return resultList;
        }

        [Transaction]
        public virtual IList<CultureHierarchyEntity> FindAllCultureHierarchies()
        {
            var resultList = m_cultureHierarchyRepository.FindAllCultureHierarchies();

            return resultList;
        }
    }
}
