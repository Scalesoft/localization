namespace Localization.Database.Abstractions.Entity
{
    public interface IIntervalText
    {
        int Id { get; }
        int IntervalStart { get; }
        int IntervalEnd { get; } 
        string Text { get; }
        int PluralizedStaticTextId { get; }
        IPluralizedStaticText PluralizedStaticText { get; set; }
    }
}