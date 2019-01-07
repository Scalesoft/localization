using System.Globalization;
using Localization.AspNetCore.Service.Manager;

namespace Localization.AspNetCore.Service.Service
{
    public abstract class ServiceBase
    {
        protected readonly RequestCultureManager m_requestCultureManager;

        protected ServiceBase(
            RequestCultureManager requestCultureManager
        )
        {
            m_requestCultureManager = requestCultureManager;
        }

        protected CultureInfo GetRequestCulture(CultureInfo defaultCulture)
        {
            return m_requestCultureManager.ResolveRequestCulture(defaultCulture);
        }
    }
}
