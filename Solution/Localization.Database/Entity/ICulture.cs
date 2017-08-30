using System.Collections.Generic;

namespace Localization.Database.Abstractions.Entity
{
    public interface ICulture
    {
        int Id { get; }
        string Name { get; }
    }
}