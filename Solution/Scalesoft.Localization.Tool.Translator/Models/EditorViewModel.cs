using System.Collections.Generic;
using System.IO;

namespace Scalesoft.Localization.Tool.Translator.Models
{
    public class EditorViewModel
    {
        public string Scope { get; set; }
        public string[] Cultures { get; set; }
        public FileInfo[] Files { get; set; }
        public IList<EditorRowViewModel> StandardRows { get; set; }
        public IList<EditorRowViewModel> ConstantRows { get; set; }
        public dynamic[] Pluralizations { get; set; }
    }

    public class EditorRowViewModel
    {
        public string Key { get; set; }
        public string[] Translations { get; set; }
    }
}