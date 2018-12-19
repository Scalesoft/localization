using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class CultureEntity : ICulture
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }
}
