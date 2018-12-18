using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class CultureHierarchyEntity : ICultureHierarchy
    {
        public virtual int Id { get; set; }

        public virtual ICulture Culture { get; set; }

        public virtual ICulture ParentCulture { get; set; }

        public virtual byte LevelProperty { get; set; }
    }
}