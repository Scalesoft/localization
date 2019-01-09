using System;
using Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Entity
{
    public class CultureHierarchyEntity : IEquatable<CultureHierarchyEntity>, ICultureHierarchy
    {
        public virtual int Id { get; set; }

        public virtual ICulture Culture { get; set; }

        public virtual ICulture ParentCulture { get; set; }

        public virtual byte LevelProperty { get; set; }

        public virtual bool Equals(CultureHierarchyEntity other)
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
            return Equals((CultureHierarchyEntity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}