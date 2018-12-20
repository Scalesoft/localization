using System;
using Localization.CoreLibrary.Common;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service.Factory
{
    /// <summary>
    ///     AttributeStringLocalizerFactory for creating IStringLocalizer instances
    /// </summary>
    public class AttributeStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IAutoLocalizationManager m_autoLocalizationManager;
        private readonly IAutoDictionaryManager m_dictionaryManager;
        private readonly IHttpContextAccessor m_httpContextAccessor;

        public AttributeStringLocalizerFactory(
            IHttpContextAccessor httpContextAccessor,
            IAutoDictionaryManager autoDictionaryManager,
            IAutoLocalizationManager autoLocalizationManager
        )
        {
            m_dictionaryManager = autoDictionaryManager;
            m_autoLocalizationManager = autoLocalizationManager;
            m_httpContextAccessor = httpContextAccessor;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            Guard.ArgumentNotNull(nameof(resourceSource), resourceSource);

            return new AttributeStringLocalizer(
                m_dictionaryManager, m_httpContextAccessor, m_autoLocalizationManager,
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
                    m_dictionaryManager, m_httpContextAccessor, m_autoLocalizationManager, baseName, parsedLocation
                );
            }

            throw new ArgumentException($"Location string \"{location}\" is not valid enum.");
        }
    }
}