using System;

namespace Localization.CoreLibrary.Entity
{
    public class DynamicText
    {
        public string Name { get; set; }

        public short Format { get; set; }

        public bool FallBack { get; set; }

        public DateTime ModificationTime { get; set; }

        public string ModificationUser { get; set; }

        public string DictionaryScope { get; set; }

        public string Text { get; set; }

        public string Culture { get; set; }
    }
}