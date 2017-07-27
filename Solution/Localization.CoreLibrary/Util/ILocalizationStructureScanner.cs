using System.Collections.Generic;
using System.Globalization;

namespace Localization.CoreLibrary.Util
{
    public interface ILocalizationStructureScanner
    {
        IList<CultureInfo> ScanForLocalizationFiles(IConfiguration configuration);
        IList<CultureInfo> ScanForLocalizationFiles(string path);
    }
}