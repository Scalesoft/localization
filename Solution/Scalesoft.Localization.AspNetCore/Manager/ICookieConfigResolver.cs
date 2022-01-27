using Microsoft.AspNetCore.Http;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public interface ICookieConfigResolver
    {
        /// Typically used to determine user consent for preferential cookies
        bool IsCookieAllowed(HttpRequest request);
    }
}
