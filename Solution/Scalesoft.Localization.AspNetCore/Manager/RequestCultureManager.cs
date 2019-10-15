using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Scalesoft.Localization.AspNetCore.Models;
using Scalesoft.Localization.Core.Manager;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class RequestCultureManager : IRequestCultureManager
    {
        protected const string CultureCookieName = "Localization.Culture";

        private readonly IHttpContextAccessor m_httpContextAccessor;
        private readonly IAutoLocalizationManager m_autoLocalizationManager;

        public RequestCultureManager(
            IHttpContextAccessor httpContextAccessor,
            IAutoLocalizationManager autoLocalizationManager
        )
        {
            m_httpContextAccessor = httpContextAccessor;
            m_autoLocalizationManager = autoLocalizationManager;
        }

        public CultureInfo ResolveRequestCulture(CultureInfo defaultCulture)
        {
            var request = m_httpContextAccessor.HttpContext.Request;

            var cultureCookie = request.Cookies[CultureCookieName];
            string cookieCurrentCulture = null;
            if (!string.IsNullOrEmpty(cultureCookie))
            {
                var currentCookieDeserialized = SerializationManager.DeserializeCookie(cultureCookie);

                if (currentCookieDeserialized.Success)
                {
                    cookieCurrentCulture = currentCookieDeserialized.Value.CurrentCulture;
                }
            }

            var cultureValue = cookieCurrentCulture ?? defaultCulture.Name;

            return new CultureInfo(cultureValue);
        }

        public void SetOrFixDefaultCookie()
        {
            SetCookieValues();
        }

        public void SetCulture(string culture)
        {
            SetCookieValues(culture);
        }

        private void SetCookieValues(string culture = null)
        {
            var response = m_httpContextAccessor.HttpContext.Response;
            var request = m_httpContextAccessor.HttpContext.Request;

            var currentCultureCookie = request.Cookies[CultureCookieName];

            string currentCultureName = null;

            if (culture != null)
            {
                var cultureInfo = new CultureInfo(culture);

                currentCultureName = cultureInfo.Name;
            }

            if (string.IsNullOrEmpty(currentCultureCookie))
            {
                SetLanguageCookie(response, currentCultureName);
            }
            else
            {
                var deserializationResult = SerializationManager.DeserializeCookie(currentCultureCookie);

                var cultureMustBeSet = currentCultureName != null &&
                                       deserializationResult.Success &&
                                       deserializationResult.Value.CurrentCulture != currentCultureName;

                if (!deserializationResult.Success || cultureMustBeSet)
                {
                    SetLanguageCookie(response, currentCultureName);
                }
            }
        }

        private void SetCookieValue(HttpResponse response, LocalizationCookie cookie)
        {
            var serializedCookie = SerializationManager.SerializeCookie(cookie);

            SetCookieValue(response, serializedCookie);
        }

        private void SetLanguageCookie(HttpResponse response, string culture = null)
        {
            var newCookie = new LocalizationCookie
            {
                CurrentCulture = culture,
                DefaultCulture = m_autoLocalizationManager.GetDefaultCulture().Name,
            };

            SetCookieValue(response, newCookie);
        }

        private void SetCookieValue(HttpResponse response, string value)
        {
            response.Cookies.Append(
                CultureCookieName,
                value,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                }
            );
        }
    }
}