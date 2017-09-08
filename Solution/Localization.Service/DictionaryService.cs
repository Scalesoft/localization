using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Pluralization;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.Service
{
    public class DictionaryService : ServiceBase, IDictionary
    {
        private readonly IAutoDictionaryManager m_dictionaryManager;

        public DictionaryService(IAutoDictionaryManager dictionaryManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            m_dictionaryManager = dictionaryManager;
        }

        public Dictionary<string, LocalizedString> GetDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_dictionaryManager.GetDictionary(translationSource, requestCulture, scope);
        }

        public Dictionary<string, PluralizedString> GetPluralizedDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_dictionaryManager.GetPluralizedDictionary(translationSource, requestCulture, scope);
        }

        public Dictionary<string, LocalizedString> GetConstantsDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_dictionaryManager.GetConstantsDictionary(translationSource, requestCulture, scope);
        }

        protected CultureInfo RequestCulture()
        {
            HttpRequest request = HttpContextAccessor.HttpContext.Request;

            string cultureCookie = request.Cookies[ServiceBase.CultureCookieName];
            if (cultureCookie == null)
            {
                cultureCookie = m_dictionaryManager.DefaultCulture().Name;
            }

            return new CultureInfo(cultureCookie);
        }

        
    }
}