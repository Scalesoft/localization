using Localization.Database.NHibernate.Entity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Localization.Database.NHibernate.Mappings
{
    public class DictionaryScopeMapping : ClassMapping<DictionaryScopeEntity>, IMapping
    {
        public DictionaryScopeMapping()
        {
            Table("`DictionaryScope`");

            Id(x => x.Id, map => map.Generator(Generators.Identity));

            Property(x => x.Name, map => map.NotNullable(true));
        }
    }
}
