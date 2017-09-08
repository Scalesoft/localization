using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Localization.Service
{
    public class LocalizationService : ServiceBase, ILocalization
    {       
        private readonly IAutoLocalizationManager m_localizationManager;

        public LocalizationService(IAutoLocalizationManager localizationManager, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            m_localizationManager = localizationManager;
        }

        protected CultureInfo RequestCulture()
        {
            HttpRequest request = HttpContextAccessor.HttpContext.Request;

            string cultureCookie = request.Cookies[ServiceBase.CultureCookieName];
            if (cultureCookie == null)
            {
                cultureCookie = m_localizationManager.DefaultCulture().Name;
            }

            return new CultureInfo(cultureCookie);
        }

        public LocalizedString Translate(string text, string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_localizationManager.Translate(translationSource, text, requestCulture, scope);
        }

        public LocalizedString TranslateFormat(string text, string[] parameters,
            string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_localizationManager.TranslateFormat(translationSource, text, parameters, requestCulture, scope);
        }

        public LocalizedString TranslatePluralization(string text, int number,
            string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_localizationManager.TranslatePluralization(translationSource, text, number, requestCulture, scope);
        }

        public LocalizedString TranslateConstant(string text, string scope = null, LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_localizationManager.TranslateConstant(translationSource, text, requestCulture, scope);
        }

        public LocalizedString Translate(string text, LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            return this.Translate(text, null, translationSource);
        }

        public LocalizedString TranslateFormat(string text, string[] parameters,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            return this.TranslateFormat(text, parameters, null, translationSource);
        }

        public LocalizedString TranslatePluralization(string text, int number,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            return this.TranslatePluralization(text, number, null, translationSource);
        }

        public LocalizedString TranslateConstant(string text, LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            return this.TranslateConstant(text, null, translationSource);
        }
    }
}