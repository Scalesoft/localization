using System;
using System.Collections.Generic;
using System.Globalization;
using Scalesoft.Localization.Core.Model;

namespace Scalesoft.Localization.AspNetCore
{
    public interface IDynamicTextService
    {
        DynamicText GetDynamicText(string name, string scope);
        DynamicText SaveDynamicText(DynamicText dynamicText, IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing);
        IList<DynamicText> GetAllDynamicText(string name, string scope);
        void DeleteAllDynamicText(string name, string scope);
        void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo);
    }

    [Obsolete("IDynamicText is replaced by IDynamicTextService")]
    public interface IDynamicText { }
}
