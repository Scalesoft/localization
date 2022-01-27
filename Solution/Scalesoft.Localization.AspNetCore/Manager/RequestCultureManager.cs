using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Scalesoft.Localization.AspNetCore.Models;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Manager;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class RequestCultureManager : IRequestCultureManager
    {
        public const string CultureCookieName = "Localization.Culture";
        protected const string ResolvedCultureCachedItem = "ResolvedCulture";

        private readonly IHttpContextAccessor m_httpContextAccessor;
        private readonly IAutoLocalizationManager m_autoLocalizationManager;
        private readonly ICookieConfigResolver m_cookieConfigResolver;
        private readonly LocalizationConfiguration m_configuration;

        public RequestCultureManager(
            IHttpContextAccessor httpContextAccessor,
            IAutoLocalizationManager autoLocalizationManager,
            ICookieConfigResolver cookieConfigResolver,
            LocalizationConfiguration configuration
        )
        {
            m_httpContextAccessor = httpContextAccessor;
            m_autoLocalizationManager = autoLocalizationManager;
            m_cookieConfigResolver = cookieConfigResolver;
            m_configuration = configuration;
        }

        public CultureInfo ResolveRequestCulture(CultureInfo defaultCulture)
        {
            if (m_httpContextAccessor.HttpContext.Items.TryGetValue(ResolvedCultureCachedItem,
                    out var resolvedCulture) &&
                resolvedCulture is CultureInfo resolvedCultureInfo)
            {
                return resolvedCultureInfo;
            }

            var localizationCookie = GetCookieValue();

            var cultureValue = localizationCookie?.CurrentCulture ?? defaultCulture.Name;
            var cultureInfo = new CultureInfo(cultureValue);

            m_httpContextAccessor.HttpContext.Items[ResolvedCultureCachedItem] = cultureInfo;

            return cultureInfo;
        }

        public void SetResponseCookie()
        {
            BuildCookieModelAndSetCookie();
        }

        public void SetCulture(string culture)
        {
            BuildCookieModelAndSetCookie(culture);
        }

        private void BuildCookieModelAndSetCookie(string culture = null)
        {
            var preferentialAllowed = m_cookieConfigResolver.IsCookieAllowed(m_httpContextAccessor.HttpContext.Request);
            if (!preferentialAllowed)
            {
                return;
            }

            var localizationCookie = GetCookieValue();
            if (localizationCookie == null)
            {
                return;
            }

            // Set new current culture if specified
            if (culture != null)
            {
                var requestCulture = new RequestCulture(culture);
                localizationCookie.CurrentCulture = requestCulture.Culture.Name;
            }
            else
            {
                // Else set to default culture
                localizationCookie.CurrentCulture = m_autoLocalizationManager.GetDefaultCulture().Name;
            }

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
                    Secure = m_configuration.SecureCookie,
                }
            );
        }
    }
}
