using System;
using DryIoc.Facilities.NHibernate;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public abstract class BaseTextUoW : UnitOfWorkBase
    {
        protected readonly CultureRepository CultureRepository;
        protected readonly DictionaryScopeRepository DictionaryScopeRepository;

        protected BaseTextUoW(
            CultureRepository cultureRepository,
            DictionaryScopeRepository dictionaryScopeRepository,
            ISessionManager sessionManager
        ) : base(sessionManager)
        {
            CultureRepository = cultureRepository;
            DictionaryScopeRepository = dictionaryScopeRepository;
        }

        protected T AddBaseText<T>(
            string name,
            short format,
            string text,
            string cultureName,
            string dictionaryScope,
            string modificationUser,
            DateTime modificationTime
        ) where T : BaseTextEntity, new()
        {
            var culture = CultureRepository.GetCultureByName(cultureName);
            var scope = DictionaryScopeRepository.GetScopeByName(dictionaryScope);

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
