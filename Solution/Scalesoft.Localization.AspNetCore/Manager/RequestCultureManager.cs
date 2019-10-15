using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Scalesoft.Localization.AspNetCore.Models;
using Scalesoft.Localization.Core.Exception;
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

        public void SetDefaultCookie()
        {
            var response = m_httpContextAccessor.HttpContext.Response;
            var request = m_httpContextAccessor.HttpContext.Request;

            var currentCultureCookie = request.Cookies[CultureCookieName];

            if (string.IsNullOrEmpty(currentCultureCookie))
            {
                SetDefaultLanguageCookie(response);
            }
            else
            {
                var deserializationResult = SerializationManager.DeserializeCookie(currentCultureCookie);
                if (!deserializationResult.Success)
                {
                    SetDefaultLanguageCookie(response);
                }
            }
        }

        public void SetCulture(string culture)
        {
            var requestCulture = new RequestCulture(culture);
            var response = m_httpContextAccessor.HttpContext.Response;
            var request = m_httpContextAccessor.HttpContext.Request;

            var currentCultureCookie = request.Cookies[CultureCookieName];

            var currentCultureName = requestCulture.Culture.Name;

            if (string.IsNullOrEmpty(currentCultureCookie))
            {
                var newCookie = new LocalizationCookie
                {
                    CurrentCulture = currentCultureName,
                    DefaultCulture = m_autoLocalizationManager.GetDefaultCulture().Name,
                };

                SetCookieValue(response, newCookie);
            }
            else
            {
                var currentCookieDeserializedResult = SerializationManager.DeserializeCookie(currentCultureCookie);
                if (!currentCookieDeserializedResult.Success)
                {
                    throw new LocalizationManagerException("Failed to deserialize localization cookie");
                }

                var currentCookieDeserialized = currentCookieDeserializedResult.Value;

                currentCookieDeserialized.CurrentCulture = currentCultureName;

                SetCookieValue(response, currentCookieDeserialized);
            }
        }

        private void SetCookieValue(HttpResponse response, LocalizationCookie cookie)
        {
            var serializedCookie = SerializationManager.SerializeCookie(cookie);

            SetCookieValue(response, serializedCookie);
        }

        private void SetDefaultLanguageCookie(HttpResponse response)
        {
            var newCookie = new LocalizationCookie
            {
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