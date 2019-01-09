using System.Collections.Generic;
using System.Globalization;
using Scalesoft.Localization.Core.Configuration;

namespace Scalesoft.Localization.Core.Resolver
{
    public class FallbackCultureResolver
    {
        private readonly IDictionary<CultureInfo, CultureInfo> m_cultureFallback;

        public FallbackCultureResolver(LocalizationConfiguration configuration)
        {
            m_cultureFallback = new Dictionary<CultureInfo, CultureInfo>();

            var availableCulture = new List<CultureInfo>
            {
                configuration.DefaultCulture
            };

            foreach (var supportedCulture in configuration.SupportedCultures)
            {
                if (!availableCulture.Contains(supportedCulture))
                {
                    availableCulture.Add(supportedCulture);
                }
            }

            foreach (var cultureInfo in availableCulture)
            {
                var parentCulture = cultureInfo.Parent;

                if (
                    parentCulture.Equals(CultureInfo.InvariantCulture)
                    || !availableCulture.Contains(parentCulture)
                )
                {
                    parentCulture = configuration.DefaultCulture;
                }

                if (!cultureInfo.Equals(parentCulture))
                {
                    m_cultureFallback.Add(cultureInfo, parentCulture);
                }
            }
        }

        public CultureInfo FallbackCulture(CultureInfo cultureInfo)
        {
            return m_cultureFallback.TryGetValue(cultureInfo, out var fallbackCulture)
                ? fallbackCulture
                : null;
        }
    }
}
