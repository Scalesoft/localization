using System.Collections.Generic;

namespace Scalesoft.Localization.AspNetCore.Sample.Models
{
    public class DynamicTextResult
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string DirectTranslation { get; set; }
        public IList<CultureAndTextResult> AllDynamicTexts { get; set; }
    }
}