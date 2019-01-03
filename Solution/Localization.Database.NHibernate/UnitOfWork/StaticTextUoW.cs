using System;
using System.Collections.Generic;
using DryIoc.Facilities.NHibernate;
using DryIoc.Transactions;
using Localization.Database.NHibernate.Entity;
using Localization.Database.NHibernate.Repository;

namespace Localization.Database.NHibernate.UnitOfWork
{
    public class StaticTextUoW : BaseTextUoW
    {
        private readonly StaticTextRepository m_staticTextRepository;

        public StaticTextUoW(
            StaticTextRepository staticTextRepository,
            CultureRepository cultureRepository,
            DictionaryScopeRepository dictionaryScopeRepository,
            ISessionManager sessionManager
        ) : base(cultureRepository, dictionaryScopeRepository, sessionManager)
        {
            m_staticTextRepository = staticTextRepository;
        }

        [Transaction]
        public virtual int AddStaticText(
            string name,
            short format,
            string text,
            string cultureName,
            string dictionaryScope,
            string modificationUser,
            DateTime modificationTime
        )
        {
            var staticText = AddBaseText<StaticTextEntity>(
                name,
                format,
                text,
                cultureName,
                dictionaryScope,
                modificationUser,
                modificationTime
            );

            var result = (int) m_staticTextRepository.Create(staticText);

            return result;
        }

        [Transaction]
        public virtual void UpdateStaticText(
            string name,
            string cultureName,
            string dictionaryScopeName,
            short format,
            string text,
            string modificationUser,
            DateTime modificationTime
        )
        {
            var staticTextEntity = m_staticTextRepository.GetByNameAndCultureAndScope(
                name, cultureName, dictionaryScopeName
            );

            staticTextEntity.Format = format;
            staticTextEntity.Text = text;
            staticTextEntity.ModificationUser = modificationUser;
            staticTextEntity.ModificationTime = modificationTime;

            m_staticTextRepository.Update(staticTextEntity);
        }

        [Transaction]
        public virtual void Delete(
            string name,
            string cultureName,
            string dictionaryScopeName
        )
        {
            var staticTextEntity = m_staticTextRepository.GetByNameAndCultureAndScope(
                name, cultureName, dictionaryScopeName
            );

            m_staticTextRepository.Delete(staticTextEntity);
        }

        [Transaction]
        public virtual void DeleteAll(
            string name,
            string dictionaryScopeName
        )
        {
            var staticTextEntities = m_staticTextRepository.FindByNameAndScope(
                name, dictionaryScopeName
            );

            m_staticTextRepository.DeleteAll(staticTextEntities);
        }

        [Transaction]
        public virtual StaticTextEntity GetStaticTextById(int id)
        {
            var result = m_staticTextRepository.GetStaticTextById(id);

            return result;
        }

        [Transaction]
        public virtual StaticTextEntity GetByNameAndCultureAndScope(
            string name, string cultureName, string dictionaryScopeName
        )
        {
            var result = m_staticTextRepository.GetByNameAndCultureAndScope(
                name, cultureName, dictionaryScopeName
            );

            return result;
        }

        [Transaction]
        public virtual IList<StaticTextEntity> FindByNameAndScope(
            string name, string dictionaryScopeName
        )
        {
            var resultList = m_staticTextRepository.FindByNameAndScope(
                name, dictionaryScopeName
            );

            return resultList;
        }

        [Transaction]
        public virtual IList<StaticTextEntity> FindAllStaticTexts()
        {
            var resultList = m_staticTextRepository.FindAllStaticTexts();

            return resultList;
        }
    }
}
