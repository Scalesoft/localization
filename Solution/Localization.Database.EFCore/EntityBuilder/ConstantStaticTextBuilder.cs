using Localization.Database.EFCore.Entity;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class ConstantStaticTextBuilder : ITextBuilder<ConstantStaticText>
    {
        private readonly BaseTextBuilder<ConstantStaticText> m_baseTextBuilder;

        public ConstantStaticTextBuilder()
        {
            m_baseTextBuilder = new BaseTextBuilder<ConstantStaticText>(new ConstantStaticText());
        }

        public ITextBuilder<ConstantStaticText> Name(string name)
        {
            return m_baseTextBuilder.Name(name);
        }

        public ITextBuilder<ConstantStaticText> Format(short format)
        {
            return m_baseTextBuilder.Format(format);
        }

        public ITextBuilder<ConstantStaticText> ModificationUser(string modificationUser)
        {
            return m_baseTextBuilder.ModificationUser(modificationUser);
        }

        public ITextBuilder<ConstantStaticText> DictionaryScope(DictionaryScope dictionaryScope)
        {
            return m_baseTextBuilder.DictionaryScope(dictionaryScope);
        }

        public ITextBuilder<ConstantStaticText> Culture(Culture culture)
        {
            return m_baseTextBuilder.Culture(culture);
        }

        public ITextBuilder<ConstantStaticText> Text(string text)
        {
            return m_baseTextBuilder.Text(text);
        }

        public ConstantStaticText Build()
        {
            return m_baseTextBuilder.Build();
        }
    }
}