using Microsoft.AspNetCore.Http;

namespace Localization.AspNetCore.Service
{
    public abstract class ServiceBase
    {
        protected const string CultureCookieName = "Localization.Culture";

        protected readonly IHttpContextAccessor HttpContextAccessor;

        protected ServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
    }
}