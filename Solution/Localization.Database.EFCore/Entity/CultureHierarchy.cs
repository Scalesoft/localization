using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Localization.Database.Abstractions.Entity;

namespace Localization.Database.EFCore.Entity
{
    public sealed class CultureHierarchy : ICultureHierarchy
    {
        public int Id { get; set; }

        public int CultureId { get; set; }

        
        public int ParentCultureId { get; set; }

        [Required]
        public Culture Culture { get; set; }

        [Required]
        public Culture ParentCulture { get; set; }

        [Required]
        [Column(TypeName = "tinyint")]
        public byte LevelProperty { get; set; }

        ICulture ICultureHierarchy.Culture
        {
            get;
            set;
        }

        ICulture ICultureHierarchy.ParentCulture { get; set; }
    }
}