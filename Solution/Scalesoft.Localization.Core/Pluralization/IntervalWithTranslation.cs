using Microsoft.Extensions.Localization;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class IntervalWithTranslation
    {
        public PluralizationInterval Interval { get; set; }
        public LocalizedString LocalizedString { get; set; }
    }
}