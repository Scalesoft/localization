using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class StaticTextMapping : SubclassMapping<StaticTextEntity>, IMapping
    {
        public StaticTextMapping()
        {
            DiscriminatorValue("StaticText");
        }
    }
}
