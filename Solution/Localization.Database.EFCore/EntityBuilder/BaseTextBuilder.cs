using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.EntityBuilder.Common;
using Localization.Database.EFCore.Exception;
using Localization.Database.EFCore.Logging;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class BaseTextBuilder<T> : ITextBuilder<T> where T : BaseText
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly T m_baseText;

        public BaseTextBuilder(T baseText)
        {
            m_baseText = baseText;
        }

        public T CurrentInstance()
        {
            return m_baseText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Static text name.</param>
        /// <returns>this</returns>
        /// <exception cref="BuilderException">Thrown if Name is already set.</exception>
        public ITextBuilder<T> Name(string name)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(name), m_baseText.Name, Logger);

            m_baseText.Name = name;

            return this;
        }

        public ITextBuilder<T> Format(short format)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(format), m_baseText.Format, Logger);

            m_baseText.Format = format;

            return this;
        }

        public ITextBuilder<T> ModificationUser(string modificationUser)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(modificationUser), m_baseText.ModificationUser, Logger);

            m_baseText.ModificationUser = modificationUser;

            return this;
        }

        public ITextBuilder<T> DictionaryScope(DictionaryScope dictionaryScope)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(dictionaryScope), m_baseText.DictionaryScope, Logger);

            m_baseText.DictionaryScopeId = dictionaryScope.Id;
            m_baseText.DictionaryScope = dictionaryScope;
            return this;
        }

        public ITextBuilder<T> Culture(Culture culture)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(culture), m_baseText.Culture, Logger);

            m_baseText.CultureId = culture.Id;
            m_baseText.Culture = culture;
            return this;
        }

        public ITextBuilder<T> Text(string text)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(text), m_baseText.Text, Logger);

            m_baseText.Text = text;
            return this;
        }

        public T Build()
        {
            NullCheck.Check(m_baseText.DictionaryScope);
            NullCheck.Check(m_baseText.Culture);
            NullCheck.Check(m_baseText.ModificationUser);
            NullCheck.Check(m_baseText.Text);

            return m_baseText;
        }
    }
}