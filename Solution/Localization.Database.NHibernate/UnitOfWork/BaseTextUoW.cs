using System;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;
using NHibernate;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public abstract class BaseTextUoW : UnitOfWorkBase
    {
        protected BaseTextUoW(
            ISessionFactory sessionFactory
        ) : base(sessionFactory)
        {
        }

        protected T AddBaseText<T>(
            ISession session,
            string name,
            short format,
            string text,
            string cultureName,
            string dictionaryScope,
            string modificationUser,
            DateTime modificationTime
        ) where T : BaseTextEntity, new()
        {
            var cultureRepository = new CultureRepository(session);
            var dictionaryScopeRepository = new DictionaryScopeRepository(session);

            var culture = cultureRepository.GetCultureByName(cultureName);
            var scope = dictionaryScopeRepository.GetScopeByName(dictionaryScope);

            var baseTextEntity = new T
            {
                Culture = culture,
                DictionaryScope = scope,
                Name = name,
                Format = format,
                Text = text,
                ModificationTime = modificationTime,
                ModificationUser = modificationUser,
            };

            return baseTextEntity;
        }
    }
}
