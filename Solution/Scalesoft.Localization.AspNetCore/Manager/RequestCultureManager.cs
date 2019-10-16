using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Scalesoft.Localization.AspNetCore.Models;
using Scalesoft.Localization.Core.Manager;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class RequestCultureManager : IRequestCultureManager
    {
        protected const string CultureCookieName = "Localization.Culture";
        protected const string ResolvedCultureCachedItem = "ResolvedCulture";

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
            if (m_httpContextAccessor.HttpContext.Items.TryGetValue(ResolvedCultureCachedItem, out var resolvedCulture) &&
                resolvedCulture is CultureInfo resolvedCultureInfo)
            {
                return resolvedCultureInfo;
            }

            var localizationCookie = GetCookieValue();
            
            var cultureValue = localizationCookie.CurrentCulture ?? defaultCulture.Name;
            var cultureInfo = new CultureInfo(cultureValue);

            m_httpContextAccessor.HttpContext.Items[ResolvedCultureCachedItem] = cultureInfo;

            return cultureInfo;
        }

        public void SetResponseCookie()
        {
            SetCookieValues();
        }

        public void SetCulture(string culture)
        {
            SetCookieValues(culture);
        }

        private void SetCookieValues(string culture = null)
        {
            var localizationCookie = GetCookieValue();
            
            if (culture != null)
            {
                var requestCulture = new RequestCulture(culture);
                localizationCookie.CurrentCulture = requestCulture.Culture.Name;
            }

            localizationCookie.DefaultCulture = m_autoLocalizationManager.GetDefaultCulture().Name;
            
            SetCookieValue(localizationCookie);
        }

        private LocalizationCookie GetCookieValue()
        {
            var request = m_httpContextAccessor.HttpContext.Request;
            var currentCultureCookie = request.Cookies[CultureCookieName];
            var deserializedCookie = CookieSerializer.Deserialize(currentCultureCookie);
            return deserializedCookie;
        }
        
        private void SetCookieValue(LocalizationCookie cookie)
        {
            var response = m_httpContextAccessor.HttpContext.Response;
            var serializedCookie = CookieSerializer.Serialize(cookie);

            response.Cookies.Append(
                CultureCookieName,
                serializedCookie,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                }
            );
        }
    }
}