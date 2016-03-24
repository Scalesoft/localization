using System.Globalization;

namespace Localization
{
    public interface ILocalizationRepository
    {
        CultureInfo GetCurrentCultureInfo();

        void SetCurrentCultureInfo(CultureInfo cultureInfo);
    }
}