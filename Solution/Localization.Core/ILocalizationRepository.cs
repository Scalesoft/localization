using System.Globalization;

namespace Localization.Core
{
    public interface ILocalizationRepository
    {
        CultureInfo GetCurrentCultureInfo();

        void SetCurrentCultureInfo(CultureInfo cultureInfo);
    }
}