using Scalesoft.Localization.Database.EFCore.Entity;

namespace Scalesoft.Localization.Database.EFCore.EntityBuilder
{
    public class StaticTextBuilder : ITextBuilder<StaticText>
    {
        private readonly BaseTextBuilder<StaticText> m_baseTextBuilder;

        public StaticTextBuilder()
        {
            m_baseTextBuilder = new BaseTextBuilder<StaticText>(new StaticText());
        }

        public ITextBuilder<StaticText> Name(string name)
        {
            return m_baseTextBuilder.Name(name);
        }

        public ITextBuilder<StaticText> Format(short format)
        {
            return m_baseTextBuilder.Format(format);
        }

        public ITextBuilder<StaticText> ModificationUser(string modificationUser)
        {
            return m_baseTextBuilder.ModificationUser(modificationUser);
        }

        public ITextBuilder<StaticText> DictionaryScope(DictionaryScope dictionaryScope)
        {
            return m_baseTextBuilder.DictionaryScope(dictionaryScope);
        }

        public ITextBuilder<StaticText> Culture(Culture culture)
        {
            return m_baseTextBuilder.Culture(culture);
        }

        public ITextBuilder<StaticText> Text(string text)
        {
            return m_baseTextBuilder.Text(text);
        }

        public StaticText Build()
        {
            return m_baseTextBuilder.Build();
        }
    }
}