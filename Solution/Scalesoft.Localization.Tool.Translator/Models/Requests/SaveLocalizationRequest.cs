using System.Collections.Generic;

namespace Scalesoft.Localization.Tool.Translator.Models.Requests
{
    public class SaveLocalizationRequest
    {
        public string Scope { get; set; }
        public IList<TranslationItemChangeRequest> Dictionary { get; set; }
        public IList<TranslationItemChangeRequest> Constants { get; set; }
    }

    public class TranslationItemChangeRequest
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}