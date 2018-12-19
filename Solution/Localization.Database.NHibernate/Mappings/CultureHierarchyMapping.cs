using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class CultureHierarchyMapping : ClassMapping<CultureHierarchyEntity>
    {
        public CultureHierarchyMapping()
        {
            Table("`CultureHierarchy`");

            Id(x => x.Id, map => map.Generator(Generators.Identity));

            Property(x => x.LevelProperty, map => map.NotNullable(true));

            ManyToOne(x => x.Culture, map =>
            {
                map.NotNullable(true);
                map.Class(typeof(CultureEntity));
            });

            ManyToOne(x => x.ParentCulture, map =>
            {
                map.NotNullable(true);
                map.Class(typeof(CultureEntity));
            });
        }
    }
}
