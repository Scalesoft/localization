using System;
using Scalesoft.Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Entity
{
    public class IntervalTextEntity : IEquatable<IntervalTextEntity>, IIntervalText
    {
        public virtual int Id { get; set; }

        public virtual int IntervalStart { get; set; }

        public virtual int IntervalEnd { get; set; }

        public virtual string Text { get; set; }

        public virtual int PluralizedStaticTextId { get; set; }

        public virtual IPluralizedStaticText PluralizedStaticText { get; set; }

        public virtual bool Equals(IntervalTextEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IntervalTextEntity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
