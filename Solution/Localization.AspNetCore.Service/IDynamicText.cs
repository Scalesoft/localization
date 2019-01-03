using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Model;

namespace Localization.AspNetCore.Service
{
    public interface IDynamicText
    {
        DynamicText GetDynamicText(string name, string scope);
        DynamicText SaveDynamicText(DynamicText dynamicText, IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing);
        IList<DynamicText> GetAllDynamicText(string name, string scope);
        void DeleteAllDynamicText(string name, string scope);
        void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo);
    }
}
