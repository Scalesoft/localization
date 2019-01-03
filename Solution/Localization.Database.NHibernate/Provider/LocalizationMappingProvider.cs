using System;
using System.Collections.Generic;
using System.Linq;
using Localization.Database.NHibernate.Mappings;

namespace Localization.Database.NHibernate.Provider
{
    public class LocalizationMappingProvider
    {
        public IEnumerable<Type> GetMappings()
        {
            var baseType = typeof(IMapping);
            var assembly = baseType.Assembly;

            return assembly.GetExportedTypes().Where(
                t => baseType.IsAssignableFrom(t)
            );
        }
    }
}