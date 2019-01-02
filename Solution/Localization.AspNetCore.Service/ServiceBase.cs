using System.Globalization;
using Localization.AspNetCore.Service.Manager;

namespace Localization.AspNetCore.Service
{
    public abstract class ServiceBase
    {
        protected readonly RequestCultureManager RequestCultureManager;

        protected ServiceBase(
            RequestCultureManager requestCultureManager
        )
        {
            RequestCultureManager = requestCultureManager;
        }

        protected CultureInfo GetRequestCulture(CultureInfo defaultCulture)
        {
            return RequestCultureManager.ResolveRequestCulture(defaultCulture);
        }
    }
}
