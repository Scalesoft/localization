using NHibernate.Mapping.ByCode.Conformist;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Mappings
{
    public class StaticTextMapping : SubclassMapping<StaticTextEntity>, IMapping
    {
        public StaticTextMapping()
        {
            DiscriminatorValue("StaticText");
        }
    }
}
