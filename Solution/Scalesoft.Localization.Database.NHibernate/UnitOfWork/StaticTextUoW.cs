using System;
using System.Collections.Generic;
using NHibernate;
using Scalesoft.Localization.Database.Abstractions.Entity;
using Scalesoft.Localization.Database.NHibernate.Entity;
using Scalesoft.Localization.Database.NHibernate.Repository;

namespace Scalesoft.Localization.Database.NHibernate.UnitOfWork
{
    public class StaticTextUoW : BaseTextUoW
    {
        public StaticTextUoW(
            ISessionFactory sessionFactory
        ) : base(sessionFactory)
        {
        }

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
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var staticText = AddBaseText<StaticTextEntity>(
                    session,
                    name,
                    format,
                    text,
                    cultureName,
                    dictionaryScope,
                    modificationUser,
                    modificationTime
                );

                var result = (int) staticTextRepository.Create(staticText);

                return result;
            }
        }

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
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var staticTextEntity = staticTextRepository.GetByNameAndCultureAndScope(
                    name, cultureName, dictionaryScopeName
                );

                staticTextEntity.Format = format;
                staticTextEntity.Text = text;
                staticTextEntity.ModificationUser = modificationUser;
                staticTextEntity.ModificationTime = modificationTime;

                staticTextRepository.Update(staticTextEntity);
                session.Flush();
            }
        }

        public virtual void Delete(
            string name,
            string cultureName,
            string dictionaryScopeName
        )
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var staticTextEntity = staticTextRepository.GetByNameAndCultureAndScope(
                    name, cultureName, dictionaryScopeName
                );

                staticTextRepository.Delete(staticTextEntity);
                session.Flush();
            }
        }

        public virtual void DeleteAll(
            string name,
            string dictionaryScopeName
        )
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var staticTextEntities = staticTextRepository.FindByNameAndScope(
                    name, dictionaryScopeName
                );

                staticTextRepository.DeleteAll(staticTextEntities);
                session.Flush();
            }
        }

        public virtual StaticTextEntity GetStaticTextById(int id)
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var result = staticTextRepository.GetStaticTextById(id);

                return result;
            }
        }

        public virtual StaticTextEntity GetByNameAndCultureAndScope(
            string name, string cultureName, string dictionaryScopeName
        )
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var result = staticTextRepository.GetByNameAndCultureAndScope(
                    name, cultureName, dictionaryScopeName
                );

                return result;
            }
        }

        public virtual IList<StaticTextEntity> FindByNameAndScope(
            string name, string dictionaryScopeName
        )
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var resultList = staticTextRepository.FindByNameAndScope(
                    name, dictionaryScopeName
                );

                return resultList;
            }
        }

        public virtual IList<StaticTextEntity> FindAllStaticTexts()
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var resultList = staticTextRepository.FindAllStaticTexts();

                return resultList;
            }
        }

        public virtual IList<StaticTextEntity> FindAllByCultureAndScope(string cultureName, string dictionaryScopeName)
        {
            using (var session = GetSession())
            {
                var staticTextRepository = new StaticTextRepository(session);

                var resultList = staticTextRepository.FindAllByCultureAndScope(cultureName, dictionaryScopeName);

                return resultList;
            }
        }
    }
}
