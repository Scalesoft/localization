using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Scalesoft.Localization.Core.Manager
{
    public interface ILocalizationManager
    {
        LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null);

        CultureInfo DefaultCulture();

        string DefaultScope();
    }
}
