using Microsoft.AspNetCore.Http;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public interface IUserCookiePreferenceResolver
    {
        bool PreferentialCookieAllowed(HttpRequest request);
    }
}
