using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.AspNetCore
{
    /// <summary>
    ///     IStringLocalizer implementation is required for supporting localization of the ViewModel attributes e.g.
    ///     [Name("stringKey")].
    /// </summary>
    public class AttributeStringLocalizer : IStringLocalizer
    {
        private readonly IAutoLocalizationManager m_autoLocalizationManager;
        private readonly string m_baseName;
        private readonly IRequestCultureManager m_requestCultureManager;
        private readonly IAutoDictionaryManager m_dictionaryManager;

        private readonly LocTranslationSource m_location;

        private CultureInfo m_currentCultureInfo;

        private AttributeStringLocalizer(
            IRequestCultureManager requestCultureManager,
            IAutoDictionaryManager dictionaryManager,
            IAutoLocalizationManager autoLocalizationManager,
            string baseName,
            LocTranslationSource location,
            CultureInfo cultureInfo
        )
        {
            m_requestCultureManager = requestCultureManager;
            m_dictionaryManager = dictionaryManager;
            m_baseName = baseName;
            m_location = location;
            m_autoLocalizationManager = autoLocalizationManager;
            m_currentCultureInfo = cultureInfo;
        }

        public AttributeStringLocalizer(
            IRequestCultureManager requestCultureManager,
            IAutoDictionaryManager dictionaryManager,
            IAutoLocalizationManager autoLocalizationManager,
            string baseName,
            LocTranslationSource location
        ) : this(requestCultureManager, dictionaryManager, autoLocalizationManager, baseName, location, null)
        {
            //Should be empty
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return m_dictionaryManager.GetDictionary(m_location, RequestCulture(), m_baseName).Values;
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new AttributeStringLocalizer(
                m_requestCultureManager, m_dictionaryManager,
                m_autoLocalizationManager, m_baseName, m_location, culture
            );
        }

        LocalizedString IStringLocalizer.this[string name] =>
            m_autoLocalizationManager.Translate(m_location, name, RequestCulture(), m_baseName);


        LocalizedString IStringLocalizer.this[string name, params object[] arguments] =>
            m_autoLocalizationManager.TranslateFormat(m_location, name, arguments, RequestCulture(), m_baseName);

        private CultureInfo RequestCulture()
        {
            return m_requestCultureManager.ResolveRequestCulture(m_dictionaryManager.GetDefaultCulture());
        }
    }
}
