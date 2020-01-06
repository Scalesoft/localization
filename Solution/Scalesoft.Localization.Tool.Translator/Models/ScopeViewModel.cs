using System.Collections.Generic;

namespace Scalesoft.Localization.Tool.Translator.Models
{
    public class ScopeViewModel
    {
        public string Scope { get; set; }
        public IList<DictionaryEnvelopeViewModel> Dictionaries { get; set; }
    }
}