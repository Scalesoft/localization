using System.Globalization;
using Localization.CoreLibrary.Entity;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseDynamicTextService
    {
        DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo);
        DynamicText SaveDynamicText(DynamicText dynamicText);
    }
}