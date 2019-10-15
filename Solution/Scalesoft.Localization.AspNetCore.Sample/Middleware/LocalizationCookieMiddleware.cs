using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Scalesoft.Localization.AspNetCore.Sample.Middleware
{
    public class LocalizationCookieMiddleware
    {
        private readonly RequestDelegate m_next;
        private readonly ILocalizationService m_localization;

        public LocalizationCookieMiddleware(RequestDelegate next, ILocalizationService localization)
        {
            m_next = next;
            m_localization = localization;
        }

        public async Task Invoke(HttpContext context)
        {
            m_localization.SetDefaultCookie();
            await m_next.Invoke(context);
        }
    }
}