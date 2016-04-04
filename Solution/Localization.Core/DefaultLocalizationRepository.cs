using System.Globalization;
using System.Threading;

namespace Localization.Core
{
    class DefaultLocalizationRepository : ILocalizationRepository
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            return CultureInfo.CurrentCulture;
        }

        public void SetCurrentCultureInfo(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}