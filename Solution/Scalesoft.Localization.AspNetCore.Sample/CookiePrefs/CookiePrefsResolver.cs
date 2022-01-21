using Microsoft.AspNetCore.Http;
using Scalesoft.Localization.AspNetCore.Manager;

namespace Scalesoft.Localization.AspNetCore.Sample.CookiePrefs
{
    public class CookiePrefsResolver : IUserCookieCategoriesResolver
    {
        public IUserCookieCategories Resolve(HttpRequest request)
        {
            return new UserCookieCategories { EssentialAllowed = true, PreferentialAllowed = true };
        }

        private class UserCookieCategories : IUserCookieCategories
        {
            public bool EssentialAllowed { get; set; }
            public bool PreferentialAllowed { get; set; }
        }
    }
}
