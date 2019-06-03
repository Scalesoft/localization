using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Scalesoft.Localization.Core.Manager
{
    public interface ILocalizationManager
    {
        LocalizedString Translate(CultureInfo cultureInfo, string scope, string text);

        LocalizedString TranslateFormat(CultureInfo cultureInfo, string scope, string text, object[] parameters);

        LocalizedString TranslatePluralization(CultureInfo cultureInfo, string scope, string text, int number);

        LocalizedString TranslateConstant(CultureInfo cultureInfo, string scope, string text);

        CultureInfo GetDefaultCulture();

        string GetDefaultScope();
    }
}
