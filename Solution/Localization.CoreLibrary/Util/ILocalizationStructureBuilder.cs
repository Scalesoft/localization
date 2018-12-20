using System.Collections.Generic;
using System.Globalization;

namespace Localization.CoreLibrary.Util
{
    public interface ILocalizationStructureBuilder
    {
        void BuildMissingMembers(ILocalizationConfiguration configuration);
        void BuildMissingMembers(string path, IList<CultureInfo> supportingCultures);
    }
}