using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.EntityBuilder.Common;
using Localization.Database.EFCore.Exception;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class BaseTextBuilder<T> : ITextBuilder<T> where T : BaseText
    {
        protected readonly T BaseText;

        public BaseTextBuilder(T baseText)
        {
            BaseText = baseText;
        }

        public T CurrentInstance()
        {
            return BaseText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Static text name.</param>
        /// <returns>this</returns>
        /// <exception cref="BuilderException">Thrown if Name is already set.</exception>
        public ITextBuilder<T> Name(string name)
        {
            if (BaseText.Name != null)
            {
                throw new BuilderException("Name is already set");
            }

            BaseText.Name = name;

            return this;
        }

        public ITextBuilder<T> Format(int format)
        {
            BaseText.Format = format;

            return this;
        }

        public ITextBuilder<T> ModificationUser(string modificationUser)
        {
            if (BaseText.ModificationUser != null)
            {
                throw new BuilderException("ModificationUser is already set");
            }

            BaseText.ModificationUser = modificationUser;

            return this;
        }

        public ITextBuilder<T> DictionaryScope(DictionaryScope dictionaryScope)
        {
            if (BaseText.DictionaryScope != null)
            {
                throw new BuilderException("DictionaryScope is already set");
            }

            BaseText.DictionaryScopeId = dictionaryScope.Id;
            BaseText.DictionaryScope = dictionaryScope;
            return this;
        }

        public ITextBuilder<T> Culture(Culture culture)
        {
            if (BaseText.Culture != null)
            {
                throw new BuilderException("Culture is already set");
            }

            BaseText.CultureId = culture.Id;
            BaseText.Culture = culture;
            return this;
        }

        public ITextBuilder<T> Text(string text)
        {
            if (BaseText.Text != null)
            {
                throw new BuilderException("Text is already set");
            }

            BaseText.Text = text;
            return this;
        }

        public T Build()
        {
            NullCheck.Check(BaseText.DictionaryScope);
            NullCheck.Check(BaseText.Culture);
            NullCheck.Check(BaseText.ModificationUser);
            NullCheck.Check(BaseText.Text);


            return BaseText;
        }
    }
}