using System.Collections.Generic;
using System.Globalization;

namespace Localization.AspNetCore.Service
{
    public interface IDynamicText
    {
        CoreLibrary.Entity.DynamicText GetDynamicText(string name, string scope);
        CoreLibrary.Entity.DynamicText SaveDynamicText(CoreLibrary.Entity.DynamicText dynamicText);
        IList<CoreLibrary.Entity.DynamicText> GetAllDynamicText(string name, string scope);
        void DeleteAllDynamicText(string name, string scope);
        void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo);
    }
}