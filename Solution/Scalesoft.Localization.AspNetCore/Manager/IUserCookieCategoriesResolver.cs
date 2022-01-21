using Microsoft.AspNetCore.Http;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public interface IUserCookieCategoriesResolver
    {
        IUserCookieCategories Resolve(HttpRequest request);
    }
}