using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager
{
    public interface IFileLocalizationManager
    {
        LocalizedString FileTranslate(string text, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString FileTranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString FileTranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null);

        LocalizedString FileTranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null);
    }
}