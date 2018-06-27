using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    /// <summary>
    /// IStringLocalizer implementation is required for supporting localization of the ViewModel attributes e.g. [Name("stringKey")].
    /// </summary>
    public class AttributeStringLocalizer : IStringLocalizer
    {
        private const string CultureCookieName = "Localization.Culture";

        public const string SourceDictionaryFileKey = "File";
        public const string SourceDictionaryDatabaseKey = "Database";
        public const string SourceDictionaryAutoKey = "Auto";

        private readonly IHttpContextAccessor m_httpContextAccessor;

        private CultureInfo m_currentCultureInfo;

        private readonly IAutoLocalizationManager m_autoLocalizationManager;
        private readonly IAutoDictionaryManager m_dictionaryManager;
        private readonly string m_baseName;
        private readonly string m_location;

        private readonly Dictionary<string, LocTranslationSource> m_sourcesDictonary = 
            new Dictionary<string, LocTranslationSource>();

        private AttributeStringLocalizer(IAutoDictionaryManager dictionaryManager, 
                                        IHttpContextAccessor httpContextAccessor, 
                                        IAutoLocalizationManager autoLocalizationManager,
                                        string baseName, string location, CultureInfo cultureInfo)
        {
            m_sourcesDictonary.Add(SourceDictionaryFileKey, LocTranslationSource.File);
            m_sourcesDictonary.Add(SourceDictionaryDatabaseKey, LocTranslationSource.Database);
            m_sourcesDictonary.Add(SourceDictionaryAutoKey, LocTranslationSource.Auto);

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
            string baseName, string location) : this(dictionaryManager, httpContextAccessor, autoLocalizationManager, baseName, location, null)
        {
            //Should be empty
        }

        
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return m_dictionaryManager.GetDictionary(ParseLocation(m_location), RequestCulture(), m_baseName).Values;
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new AttributeStringLocalizer(m_dictionaryManager, m_httpContextAccessor, m_autoLocalizationManager, m_baseName, m_location, culture);
        }

        LocalizedString IStringLocalizer.this[string name]
        {
            get { return m_autoLocalizationManager.Translate(ParseLocation(m_location), name, RequestCulture(), m_baseName); }
        }

        
        LocalizedString IStringLocalizer.this[string name, params object[] arguments]
        {
            get { return m_autoLocalizationManager.TranslateFormat(ParseLocation(m_location), name, arguments, RequestCulture(), m_baseName); }
        }

        private LocTranslationSource ParseLocation(string locationName)
        {
            return m_sourcesDictonary[locationName];
        }

        /// <summary>
        /// Culture filter method.
        /// </summary>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>If cultureInfo param is not null, paramater value is returned. Else CultureInfo from cookie.</returns>
        private CultureInfo RequestCulture(CultureInfo cultureInfo = null)
        {
            if (cultureInfo != null)
            {
                return cultureInfo;
            }

            HttpRequest request = m_httpContextAccessor.HttpContext.Request;

            string cultureCookie = request.Cookies[CultureCookieName];
            if (cultureCookie == null)
            {
                cultureCookie = m_dictionaryManager.DefaultCulture().Name;
            }

            return new CultureInfo(cultureCookie);
        }
    }
}