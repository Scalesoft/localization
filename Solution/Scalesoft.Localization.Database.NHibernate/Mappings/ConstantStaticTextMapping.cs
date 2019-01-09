using NHibernate.Mapping.ByCode.Conformist;
using Scalesoft.Localization.Database.NHibernate.Entity;

namespace Scalesoft.Localization.Database.NHibernate.Mappings
{
    public class ConstantStaticTextMapping : SubclassMapping<ConstantStaticTextEntity>, IMapping
    {
        public ConstantStaticTextMapping()
        {
            DiscriminatorValue("ConstantStaticText");
        }
    }
}
