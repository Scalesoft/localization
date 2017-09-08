using System;
using Localization.Database.EFCore.Entity;

namespace Localization.Database.EFCore.EntityBuilder
{
    public interface ITextBuilder<T>
    {
        ITextBuilder<T> Name(string name);
        ITextBuilder<T> Format(short format);
        ITextBuilder<T> ModificationUser(string modificationUser);
        ITextBuilder<T> DictionaryScope(DictionaryScope dictionaryScope);
        ITextBuilder<T> Culture(Culture culture);
        ITextBuilder<T> Text(string text);

        T Build();
    }
}