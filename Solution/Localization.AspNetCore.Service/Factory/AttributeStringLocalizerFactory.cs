using System;
using Localization.CoreLibrary.Common;
using Localization.CoreLibrary.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Localization.AspNetCore.Service.Factory
{
    /// <summary>
    /// AttributeStringLocalizerFactory for creating IStringLocalizer instances
    /// </summary>
    public class AttributeStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IAutoDictionaryManager m_dictionaryManager;
        private readonly IHttpContextAccessor m_httpContextAccessor;
        private readonly IAutoLocalizationManager m_autoLocalizationManager;

        public AttributeStringLocalizerFactory(IHttpContextAccessor httpContextAccessor)
        {
            m_dictionaryManager = Localization.CoreLibrary.Localization.Dictionary;           
            m_autoLocalizationManager = Localization.CoreLibrary.Localization.Translator;
            m_httpContextAccessor = httpContextAccessor;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            Guard.ArgumentNotNull(nameof(resourceSource), resourceSource);

            return new AttributeStringLocalizer(m_dictionaryManager, m_httpContextAccessor, m_autoLocalizationManager, resourceSource.Name, "auto");
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(baseName), baseName);
            Guard.ArgumentNotNullOrEmpty(nameof(location), location);

            return new AttributeStringLocalizer(m_dictionaryManager, m_httpContextAccessor, m_autoLocalizationManager, baseName, location);
        }
    }

}