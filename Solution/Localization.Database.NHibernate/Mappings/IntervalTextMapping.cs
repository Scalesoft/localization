using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class IntervalTextMapping : ClassMapping<IntervalTextEntity>, IMapping
    {
        public IntervalTextMapping()
        {
            Table("`IntervalText`");

            Id(x => x.Id, map => map.Generator(Generators.Identity));

            Property(x => x.IntervalStart, map => map.NotNullable(true));

            Property(x => x.IntervalEnd, map => map.NotNullable(true));

            Property(x => x.Text, map => map.NotNullable(true));

            ManyToOne(x => x.PluralizedStaticText, map =>
            {
                map.NotNullable(true);
                map.Class(typeof(PluralizedStaticTextEntity));
                map.Column("`PluralizedStaticText`");
            });
        }
    }
}
