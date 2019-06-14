using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Mappings
{
    public class CultureMapping : ClassMapping<CultureEntity>, IMapping
    {
        public CultureMapping()
        {
            Table("`Culture`");

            Id(x => x.Id, map => map.Generator(Generators.Identity));

            Property(x => x.Name, map => map.NotNullable(true));

            Bag(x => x.CultureHierarchy, colmap =>
            {
                colmap.Key(x => x.Column("Culture"));
                colmap.Inverse(true);
            }, map => { map.OneToMany(); });

            Bag(x => x.ChildCultureHierarchy, colmap =>
            {
                colmap.Key(x => x.Column("ParentCulture"));
                colmap.Inverse(true);
            }, map => { map.OneToMany(); });
        }
    }
}