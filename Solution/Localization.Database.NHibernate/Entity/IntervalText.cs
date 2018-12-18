using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class IntervalText : IIntervalText
    {
        public virtual int Id { get; set; }

        public virtual int IntervalStart { get; set; }

        public virtual int IntervalEnd { get; set; }

        public virtual string Text { get; set; }

        public virtual int PluralizedStaticTextId { get; set; }

        public virtual IPluralizedStaticText PluralizedStaticText { get; set; }
    }
}