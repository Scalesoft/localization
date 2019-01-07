using System.Collections.Generic;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;
using NHibernate;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class CultureUoW : UnitOfWorkBase
    {
        public CultureUoW(
            ISessionFactory sessionFactory
        ) : base(sessionFactory)
        {
        }

        public virtual int AddCulture(string name)
        {
            using (var session = GetSession())
            {
                var cultureRepository = new CultureRepository(session);

                var culture = new CultureEntity
                {
                    Name = name
                };

                var result = (int)cultureRepository.Create(culture);

                return result;
            }
        }

        public virtual CultureEntity GetCultureById(int id)
        {
            using (var session = GetSession())
            {
                var cultureRepository = new CultureRepository(session);

                var resultList = cultureRepository.GetCultureById(id);

                return resultList;
            }
        }

        public virtual CultureEntity GetCultureByName(string cultureName)
        {
            using (var session = GetSession())
            {
                var cultureRepository = new CultureRepository(session);

                var resultList = cultureRepository.GetCultureByName(cultureName);

                return resultList;
            }
        }

        public virtual IList<CultureEntity> FindAllCultures()
        {
            using (var session = GetSession())
            {
                var cultureRepository = new CultureRepository(session);

                var resultList = cultureRepository.FindAllCultures();

                return resultList;
            }
        }
    }
}
