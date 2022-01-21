using Microsoft.AspNetCore.Http;
using Scalesoft.Localization.AspNetCore.Manager;

namespace Scalesoft.Localization.AspNetCore.Sample.CookiePrefs
{
    public class CookiePrefsResolver : IUserCookiePreferenceResolver
    {
        public bool PreferentialCookieAllowed(HttpRequest request)
        {
            return true;
        }
    }
}
