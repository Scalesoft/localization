using Microsoft.AspNetCore.Http;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public class DefaultCookieConfigResolver : ICookieConfigResolver
    {
        public bool IsCookieAllowed(HttpRequest request)
        {
            return true;
        }
    }
}
