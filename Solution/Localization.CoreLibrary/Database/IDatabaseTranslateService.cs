using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseTranslateService
    {
        LocalizedString DatabaseTranslate(string text, CultureInfo cultureInfo, string scope);

        LocalizedString DatabaseTranslateFormat(string text, object[] parameters, CultureInfo cultureInfo, string scope);

        LocalizedString DatabaseTranslatePluralization(string text, int number, CultureInfo cultureInfo, string scope);

        LocalizedString DatabaseTranslateConstant(string text, CultureInfo cultureInfo, string scope);
    }
}