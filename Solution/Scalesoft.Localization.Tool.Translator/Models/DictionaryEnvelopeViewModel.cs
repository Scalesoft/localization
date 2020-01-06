using System.IO;
using Scalesoft.Localization.Tool.Translator.Models.Json;

namespace Scalesoft.Localization.Tool.Translator.Models
{
    public class DictionaryEnvelopeViewModel
    {
        public FileInfo FileInfo { get; set; }
        public DictionaryModel DictionaryData { get; set; }
    }
}