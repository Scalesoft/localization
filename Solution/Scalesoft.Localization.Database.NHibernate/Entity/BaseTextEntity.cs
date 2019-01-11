using System;
using Scalesoft.Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Entity
{
    public abstract class BaseTextEntity : IEquatable<BaseTextEntity>, IBaseText
    {
        public virtual int Id { get; set; }

        public virtual ICulture Culture { get; set; }

        public virtual IDictionaryScope DictionaryScope { get; set; }

        public virtual string Name { get; set; }

        public virtual short Format { get; set; }

        public virtual string Text { get; set; }

        public virtual DateTime ModificationTime { get; set; }

        public virtual string ModificationUser { get; set; }

        public virtual int CultureId => Culture.Id;

        public virtual int DictionaryScopeId => DictionaryScope.Id;

        public virtual bool Equals(BaseTextEntity other)
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
            return Equals((BaseTextEntity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
