using System;
using Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Entity
{
    public class CultureEntity : IEquatable<CultureEntity>, ICulture
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Equals(CultureEntity other)
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
            return Equals((CultureEntity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
