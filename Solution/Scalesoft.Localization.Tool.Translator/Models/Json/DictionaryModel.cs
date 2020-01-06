using System.Collections.Generic;

namespace Scalesoft.Localization.Tool.Translator.Models.Json
{
    public class DictionaryModel
    {
        public string Culture { get; set; }

        public string Scope { get; set; }

        public IDictionary<string, string> Dictionary { get; set; }

        public IDictionary<string, string> Constants { get; set; }

        public dynamic Plural { get; set; }
    }
}