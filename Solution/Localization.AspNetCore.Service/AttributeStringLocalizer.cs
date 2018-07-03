using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    /// <summary>
    ///     IStringLocalizer implementation is required for supporting localization of the ViewModel attributes e.g.
    ///     [Name("stringKey")].
    /// </summary>
    public class AttributeStringLocalizer : IStringLocalizer
    {
        private const string CultureCookieName = "Localization.Culture";

        private readonly IAutoLocalizationManager m_autoLocalizationManager;
        private readonly string m_baseName;
        private readonly IAutoDictionaryManager m_dictionaryManager;

        private readonly IHttpContextAccessor m_httpContextAccessor;
        private readonly LocTranslationSource m_location;

        private CultureInfo m_currentCultureInfo;

        private AttributeStringLocalizer(IAutoDictionaryManager dictionaryManager,
            IHttpContextAccessor httpContextAccessor,
            IAutoLocalizationManager autoLocalizationManager,
            string baseName, LocTranslationSource location, CultureInfo cultureInfo)
        {
            m_dictionaryManager = dictionaryManager;
            m_baseName = baseName;
            m_location = location;
            m_httpContextAccessor = httpContextAccessor;
            m_autoLocalizationManager = autoLocalizationManager;
            m_currentCultureInfo = cultureInfo;
        }

        public AttributeStringLocalizer(IAutoDictionaryManager dictionaryManager,
            IHttpContextAccessor httpContextAccessor,
            IAutoLocalizationManager autoLocalizationManager,
            string baseName, LocTranslationSource location) : this(dictionaryManager, httpContextAccessor,
            autoLocalizationManager, baseName, location, null)
        {
            //Should be empty
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return m_dictionaryManager.GetDictionary(m_location, RequestCulture(), m_baseName).Values;
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new AttributeStringLocalizer(m_dictionaryManager, m_httpContextAccessor, m_autoLocalizationManager,
                m_baseName, m_location, culture);
        }

        LocalizedString IStringLocalizer.this[string name] =>
            m_autoLocalizationManager.Translate(m_location, name, RequestCulture(), m_baseName);


        LocalizedString IStringLocalizer.this[string name, params object[] arguments] =>
            m_autoLocalizationManager.TranslateFormat(m_location, name, arguments, RequestCulture(), m_baseName);

        /// <summary>
        ///     Culture filter method.
        /// </summary>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>If cultureInfo param is not null, paramater value is returned. Else CultureInfo from cookie.</returns>
        private CultureInfo RequestCulture(CultureInfo cultureInfo = null)
        {
            if (cultureInfo != null) return cultureInfo;

            HttpRequest request = m_httpContextAccessor.HttpContext.Request;

            string cultureCookie = request.Cookies[CultureCookieName];
            if (cultureCookie == null) cultureCookie = m_dictionaryManager.DefaultCulture().Name;

            return new CultureInfo(cultureCookie);
        }
    }
}