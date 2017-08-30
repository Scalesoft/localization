using System;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Exception;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class DictionaryScopeBuilder
    {
        private readonly DictionaryScope m_dictionaryScope;

        public DictionaryScopeBuilder()
        {
            m_dictionaryScope = new DictionaryScope();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Dictionary scope name. E.g. global or about</param>
        /// <returns>this</returns>
        /// <exception cref="BuilderException">Thrown if Name is already set.</exception>
        public DictionaryScopeBuilder Name(string name)
        {
            if (m_dictionaryScope.Name != null)
            {
                throw new BuilderException("Name is already set");
            }

            m_dictionaryScope.Name = name;

            return this;
        }


        public IDictionaryScope Build()
        {
            if (m_dictionaryScope.Name == null)
            {
                throw new NullReferenceException("Name is not set");
            }

            return m_dictionaryScope;
        }
    }
}