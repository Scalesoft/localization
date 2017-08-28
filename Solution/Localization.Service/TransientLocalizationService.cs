using System;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Localization.CoreLibrary.Manager;
using Microsoft.AspNetCore.Http;

namespace Localization.Service
{
    class TransientLocalizationService : ILocalization
    {
        private readonly ILocalizationManager m_fileLocalizationManager;
        private readonly IHttpContextAccessor m_httpContextAccessor;

        public TransientLocalizationService(ILocalizationManager fileLocalizationManager, IHttpContextAccessor httpContextAccessor)
        {
            m_fileLocalizationManager = fileLocalizationManager;
            m_httpContextAccessor = httpContextAccessor;
        }

        private CultureInfo RequestCulture()
        {
            HttpRequest request = m_httpContextAccessor.HttpContext.Request;

            string cultureCookieName = "Localization.Culture";
            string cultureCookie = request.Cookies[cultureCookieName];
            if (cultureCookie == null)
            {
                cultureCookie = m_fileLocalizationManager.DefaultCulture().Name;
            }

            return new CultureInfo(cultureCookie);
        }

        public LocalizedString Translate(string text, string scope = null)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_fileLocalizationManager.Translate(text, requestCulture, scope);
        }

        public LocalizedString TranslateConstant(string text, string scope = null)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_fileLocalizationManager.TranslateConstant(text, requestCulture, scope);
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, string scope = null)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_fileLocalizationManager.TranslateFormat(text, parameters, requestCulture, scope);
        }

        public LocalizedString TranslatePluralization(string text, int number, string scope = null)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_fileLocalizationManager.TranslatePluralization(text, number, requestCulture, scope);
        }
    }
}
