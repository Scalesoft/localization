using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scalesoft.Localization.Tool.Translator.Models.Json
{
    public class DictionaryModel
    {
        [JsonProperty("culture")]
        public string Culture { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("dictionary")]
        public IDictionary<string, string> Dictionary { get; set; }

        [JsonProperty("constants")]
        public IDictionary<string, string> Constants { get; set; }

        [JsonProperty("plural")]
        public dynamic Plural { get; set; }
    }
}