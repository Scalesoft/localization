using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.EntityBuilder.Common;
using Localization.Database.EFCore.Exception;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class IntervalTextBuilder
    {
        private readonly IntervalText m_intervalText;

        public IntervalTextBuilder()
        {
            m_intervalText = new IntervalText();
        }

        public IntervalTextBuilder IntervalStart(int intervalStart)
        {
            m_intervalText.IntervalStart = intervalStart;

            return this;
        }

        public IntervalTextBuilder IntervalEnd(int intervalEnd)
        {
            m_intervalText.IntervalEnd = intervalEnd;

            return this;
        }

        public IntervalTextBuilder Text(string text)
        {
            if (m_intervalText.Text != null)
            {
                throw new BuilderException("Text is already set");
            }

            m_intervalText.Text = text;

            return this;
        }

        public IntervalTextBuilder PluralizedStaticText(PluralizedStaticText pluralizedStaticText)
        {
            if (m_intervalText.PluralizedStaticText != null)
            {
                throw new BuilderException("Text is already set");
            }

            NullCheck.Check(pluralizedStaticText);

            m_intervalText.PluralizedStaticText = pluralizedStaticText;
            m_intervalText.PluralizedStaticTextId = pluralizedStaticText.Id;

            return this;
        }

        public IIntervalText Build()
        {
            NullCheck.Check(m_intervalText.PluralizedStaticText);
            NullCheck.Check(m_intervalText.Text);

            return m_intervalText;
        }
    }
}