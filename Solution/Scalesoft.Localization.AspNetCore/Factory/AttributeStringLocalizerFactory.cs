using System;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.Core.Common;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.AspNetCore.Factory
{
    /// <summary>
    ///     AttributeStringLocalizerFactory for creating IStringLocalizer instances
    /// </summary>
    public class AttributeStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IAutoLocalizationManager m_autoLocalizationManager;
        private readonly IAutoDictionaryManager m_dictionaryManager;
        private readonly IRequestCultureManager m_requestCultureManager;

        public AttributeStringLocalizerFactory(
            IRequestCultureManager requestCultureManager,
            IAutoDictionaryManager autoDictionaryManager,
            IAutoLocalizationManager autoLocalizationManager
        )
        {
            m_dictionaryManager = autoDictionaryManager;
            m_autoLocalizationManager = autoLocalizationManager;
            m_requestCultureManager = requestCultureManager;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            Guard.ArgumentNotNull(nameof(resourceSource), resourceSource);

            return new AttributeStringLocalizer(
                m_requestCultureManager, m_dictionaryManager, m_autoLocalizationManager,
                resourceSource.Name, LocTranslationSource.Auto
            );
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(baseName), baseName);
            Guard.ArgumentNotNullOrEmpty(nameof(location), location);

            if (Enum.TryParse<LocTranslationSource>(location, out var parsedLocation))
            {
                return new AttributeStringLocalizer(
                    m_requestCultureManager, m_dictionaryManager, m_autoLocalizationManager, baseName, parsedLocation
                );
            }

            throw new ArgumentException($"Location string \"{location}\" is not valid enum.");
        }
    }
}
