using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Scalesoft.Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.EFCore.Entity
{
    public sealed class IntervalText : IIntervalText
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int IntervalStart { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int IntervalEnd { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Text { get; set; }

        public int PluralizedStaticTextId { get; set; }

        public PluralizedStaticText PluralizedStaticText { get; set; }

        IPluralizedStaticText IIntervalText.PluralizedStaticText { get; set; }
    }
}
