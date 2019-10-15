using Microsoft.AspNetCore.Builder;
using Scalesoft.Localization.AspNetCore.Sample.Middleware;

namespace Scalesoft.Localization.AspNetCore.Sample.Extensions
{
    public static class LocalizationCookieExtension
    {
        public static IApplicationBuilder UseLocalizationCookieSetter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LocalizationCookieMiddleware>();
        }
    }
}