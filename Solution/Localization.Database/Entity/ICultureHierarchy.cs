namespace Localization.Database.Abstractions.Entity
{
    public interface ICultureHierarchy
    {
        int Id { get; }
        ICulture Culture { get; set; }
        ICulture ParentCulture { get; set; }
        byte LevelProperty { get; }
    }
}