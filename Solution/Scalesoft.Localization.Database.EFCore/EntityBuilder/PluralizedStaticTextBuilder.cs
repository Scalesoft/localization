using System.Collections.Generic;
using Scalesoft.Localization.Database.EFCore.Entity;
using Scalesoft.Localization.Database.EFCore.EntityBuilder.Common;
using Scalesoft.Localization.Database.EFCore.Exception;

namespace Scalesoft.Localization.Database.EFCore.EntityBuilder
{
    public class PluralizedStaticTextBuilder : ITextBuilder<PluralizedStaticText>
    {
        private readonly BaseTextBuilder<PluralizedStaticText> m_baseTextBuilder;

        public PluralizedStaticTextBuilder() 
        {
            m_baseTextBuilder = new BaseTextBuilder<PluralizedStaticText>(new PluralizedStaticText());
            InstantianateIntervalTextCollectionIfNull();

        }

        private void InstantianateIntervalTextCollectionIfNull()
        {
            if (m_baseTextBuilder.CurrentInstance().IntervalTexts == null)
            {
                m_baseTextBuilder.CurrentInstance().IntervalTexts = new List<IntervalText>();
            }
        }

        public PluralizedStaticTextBuilder AddIntervalText(IntervalText intervalText)
        {
            if (m_baseTextBuilder.CurrentInstance().IntervalTexts.Contains(intervalText))
            {
                throw new BuilderException("IntervalText already exists");
            }

            NullCheck.Check(intervalText);
            NullCheck.Check(m_baseTextBuilder.CurrentInstance().IntervalTexts);

            m_baseTextBuilder.CurrentInstance().IntervalTexts.Add(intervalText);

            return this;
        }

        public ITextBuilder<PluralizedStaticText> Name(string name)
        {
            return m_baseTextBuilder.Name(name);
        }

        public ITextBuilder<PluralizedStaticText> Format(short format)
        {
            return m_baseTextBuilder.Format(format);
        }

        public ITextBuilder<PluralizedStaticText> ModificationUser(string modificationUser)
        {
            return m_baseTextBuilder.ModificationUser(modificationUser);
        }

        public ITextBuilder<PluralizedStaticText> DictionaryScope(DictionaryScope dictionaryScope)
        {
            return m_baseTextBuilder.DictionaryScope(dictionaryScope);
        }

        public ITextBuilder<PluralizedStaticText> Culture(Culture culture)
        {
            return m_baseTextBuilder.Culture(culture);
        }

        public ITextBuilder<PluralizedStaticText> Text(string text)
        {
            return m_baseTextBuilder.Text(text);
        }

        public PluralizedStaticText Build()
        {
            NullCheck.Check(m_baseTextBuilder.CurrentInstance().IntervalTexts);

            return m_baseTextBuilder.Build();
        }
    }
}