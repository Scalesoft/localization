using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.AspNetCore.Service
{
    public class AttributeStringLocalizer : IStringLocalizer
    {
        private const string CultureCookieName = "Localization.Culture";
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
            m_sourcesDictonary.Add("File", LocTranslationSource.File);
            m_sourcesDictonary.Add("Database", LocTranslationSource.Database);
            m_sourcesDictonary.Add("Auto", LocTranslationSource.Auto);

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