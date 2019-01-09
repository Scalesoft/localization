using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Mappings
{
    public class BaseTextMapping : ClassMapping<BaseTextEntity>, IMapping
    {
        public BaseTextMapping()
        {
            Table("`BaseText`");

            Id(x => x.Id, map => map.Generator(Generators.Identity));

            Discriminator(x =>
            {
                x.Force(true);
                x.NotNullable(true);
                x.Column("`Discriminator`");
            });

            Property(x => x.Name, map => map.NotNullable(true));

            Property(x => x.Format, map => map.NotNullable(true));

            Property(x => x.ModificationTime, map => map.NotNullable(true));

            Property(x => x.ModificationUser);

            Property(x => x.Text, map => map.NotNullable(true));

            ManyToOne(x => x.Culture, map =>
            {
                map.NotNullable(true);
                map.Class(typeof(CultureEntity));
                map.Column("`Culture`");
            });

            ManyToOne(x => x.DictionaryScope, map =>
            {
                map.NotNullable(true);
                map.Class(typeof(DictionaryScopeEntity));
                map.Column("`DictionaryScope`");
            });
        }
    }
}
