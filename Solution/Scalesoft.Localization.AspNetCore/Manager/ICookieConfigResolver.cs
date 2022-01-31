using Microsoft.AspNetCore.Http;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public interface ICookieConfigResolver
    {
        /// <summary>
        /// Typically used to determine user consent for preferential cookies
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool IsCookieAllowed(HttpRequest request);
    }
}
