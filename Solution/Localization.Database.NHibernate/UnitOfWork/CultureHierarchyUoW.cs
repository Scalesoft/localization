using System.Collections.Generic;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;
using NHibernate;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class CultureHierarchyUoW : UnitOfWorkBase
    {
        public CultureHierarchyUoW(
            ISessionFactory sessionFactory
        ) : base(sessionFactory)
        {
        }

        public virtual int AddCultureHierarchy(
            CultureEntity culture,
            CultureEntity parentCulture,
            byte levelProperty
        )
        {
            using (var session = GetSession())
            {
                var cultureHierarchyRepository = new CultureHierarchyRepository(session);

                var cultureHierarchy = new CultureHierarchyEntity
                {
                    Culture = culture,
                    ParentCulture = parentCulture,
                    LevelProperty = levelProperty,
                };

                var result = (int) cultureHierarchyRepository.Create(cultureHierarchy);

                return result;
            }
        }

        public virtual CultureHierarchyEntity GetCultureHierarchyById(int id)
        {
            using (var session = GetSession())
            {
                var cultureHierarchyRepository = new CultureHierarchyRepository(session);

                var resultList = cultureHierarchyRepository.GetCultureHierarchyById(id);

                return resultList;
            }
        }

        public virtual IList<CultureHierarchyEntity> FindCultureHierarchyByCulture(CultureEntity culture)
        {
            using (var session = GetSession())
            {
                var cultureHierarchyRepository = new CultureHierarchyRepository(session);

                var resultList = cultureHierarchyRepository.FindCultureHierarchyByCulture(culture);

                return resultList;
            }
        }

        public virtual IList<CultureHierarchyEntity> FindAllCultureHierarchies()
        {
            using (var session = GetSession())
            {
                var cultureHierarchyRepository = new CultureHierarchyRepository(session);

                var resultList = cultureHierarchyRepository.FindAllCultureHierarchies();

                return resultList;
            }
        }
    }
}
