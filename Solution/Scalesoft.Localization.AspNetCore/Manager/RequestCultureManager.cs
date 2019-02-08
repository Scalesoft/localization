using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class RequestCultureManager
    {
        protected const string CultureCookieName = "Localization.Culture";

        private readonly IHttpContextAccessor m_httpContextAccessor;

        public RequestCultureManager(
            IHttpContextAccessor httpContextAccessor
        )
        {
            m_httpContextAccessor = httpContextAccessor;
        }

        public CultureInfo ResolveRequestCulture(CultureInfo defaultCulture)
        {
            var request = m_httpContextAccessor.HttpContext.Request;

            var cultureCookie = request.Cookies[CultureCookieName] ?? defaultCulture.Name;

            return new CultureInfo(cultureCookie);
        }

        public void SetCulture(string culture)
        {
            var requestCulture = new RequestCulture(culture);
            var response = m_httpContextAccessor.HttpContext.Response;

            response.Cookies.Append(
                CultureCookieName,
                requestCulture.Culture.Name,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                }
            );
        }
    }
}
