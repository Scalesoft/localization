using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class DictionaryScopeEntity : IDictionaryScope
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
    }
}