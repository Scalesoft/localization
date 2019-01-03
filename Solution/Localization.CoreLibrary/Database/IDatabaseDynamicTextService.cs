using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Model;

namespace Localization.CoreLibrary.Database
{
    public interface IDatabaseDynamicTextService
    {
        DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo);
        IList<DynamicText> GetAllDynamicText(string name, string scope);
        DynamicText SaveDynamicText(DynamicText dynamicText, IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing);
        void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo);
        void DeleteAllDynamicText(string name, string scope);
    }
}
