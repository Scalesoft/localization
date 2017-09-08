using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.EntityBuilder.Common;
using Localization.Database.EFCore.Exception;
using Localization.Database.EFCore.Logging;
using Microsoft.Extensions.Logging;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class IntervalTextBuilder
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly IntervalText m_intervalText;
        private bool m_intervalStartSet = false;
        private bool m_intervalEndSet = false;


        public IntervalTextBuilder()
        {
            m_intervalText = new IntervalText();
        }

        public IntervalTextBuilder IntervalStart(int intervalStart)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(intervalStart), m_intervalStartSet, Logger);

            m_intervalText.IntervalStart = intervalStart;

            m_intervalStartSet = true;
            return this;
        }

        public IntervalTextBuilder IntervalEnd(int intervalEnd)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(intervalEnd), m_intervalEndSet, Logger);

            m_intervalText.IntervalEnd = intervalEnd;
            m_intervalEndSet = true;
            return this;
        }

        public IntervalTextBuilder Text(string text)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(text), m_intervalText.Text, Logger);

            m_intervalText.Text = text;

            return this;
        }

        public IntervalTextBuilder PluralizedStaticText(PluralizedStaticText pluralizedStaticText)
        {
            BuilderGuard.ArgumentAlreadySet(nameof(pluralizedStaticText), m_intervalText.PluralizedStaticText, Logger);

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