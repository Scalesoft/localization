using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class CultureEntity : ICulture
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        protected virtual bool Equals(CultureEntity other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CultureEntity) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
