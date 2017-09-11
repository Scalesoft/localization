namespace Localization.Database.Abstractions.Entity
{
    public interface ICultureHierarchy
    {
        int Id { get; }
        //int CultureId { get; }
        //int ParentCultureId { get; } 
        ICulture Culture { get; set; }
        ICulture ParentCulture { get; set; }
        byte LevelProperty { get; }
    }
}