using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class ConstantStaticTextMapping : SubclassMapping<ConstantStaticTextEntity>, IMapping
    {
        public ConstantStaticTextMapping()
        {
            DiscriminatorValue("ConstantStaticText");
        }
    }
}
