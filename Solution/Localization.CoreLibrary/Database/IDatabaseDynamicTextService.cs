using System.Collections.Generic;
using System.Globalization;
using Scalesoft.Localization.Core.Model;

namespace Scalesoft.Localization.Core.Database
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
