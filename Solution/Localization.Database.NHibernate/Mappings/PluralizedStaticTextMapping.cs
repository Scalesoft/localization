using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class PluralizedStaticTextMapping : SubclassMapping<PluralizedStaticTextEntity>, IMapping
    {
        public PluralizedStaticTextMapping()
        {
            DiscriminatorValue("PluralizedStaticText");

            Set(x => x.IntervalTexts, map =>
            {
                map.Table("`IntervalTexts`");
                map.Key(keyMapper => keyMapper.Column("`PluralizedStaticText`"));
                map.Inverse(true);
            }, rel => rel.OneToMany());
        }
    }
}
