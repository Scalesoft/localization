using System.Globalization;

namespace Scalesoft.Localization.AspNetCore.Manager
{
    public interface IRequestCultureManager
    {
        CultureInfo ResolveRequestCulture(CultureInfo defaultCulture);

        void SetCulture(string culture);

        void SetDefaultCookie();
    }
}