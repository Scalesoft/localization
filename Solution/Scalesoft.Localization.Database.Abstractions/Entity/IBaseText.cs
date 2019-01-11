using System;

namespace Scalesoft.Localization.Database.Abstractions.Entity
{
    public interface IBaseText
    {
        int Id { get; }
        string Name { get; }
        short Format { get; }
        DateTime ModificationTime { get; }
        string ModificationUser { get; }
        IDictionaryScope DictionaryScope { get; set; }
        int CultureId { get; }
        int DictionaryScopeId { get; }
        string Text { get; }
        ICulture Culture { get; set; }
    }
}
