using NHibernate.Mapping.ByCode.Conformist;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Mappings
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
