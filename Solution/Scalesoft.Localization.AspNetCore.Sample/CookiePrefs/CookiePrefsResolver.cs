using Microsoft.AspNetCore.Http;
using Scalesoft.Localization.AspNetCore.Manager;

namespace Scalesoft.Localization.AspNetCore.Sample.CookiePrefs
{
    public class CookiePrefsResolver : ICookieConfigResolver
    {
        public bool IsCookieAllowed(HttpRequest request)
        {
            return true;
        }
    }
}
