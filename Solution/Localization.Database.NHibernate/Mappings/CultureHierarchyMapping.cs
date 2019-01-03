using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class CultureHierarchyMapping : ClassMapping<CultureHierarchyEntity>, IMapping
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
                map.Column("Culture");
            });

            ManyToOne(x => x.ParentCulture, map =>
            {
                map.NotNullable(true);
                map.Class(typeof(CultureEntity));
                map.Column("ParentCulture");
            });
        }
    }
}
