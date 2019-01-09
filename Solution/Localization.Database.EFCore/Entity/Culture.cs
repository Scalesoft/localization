using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.EFCore.Entity
{
    public sealed class Culture : ICulture
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(5)")]
        public string Name { get; set; }


        public ICollection<BaseText> BaseTexts { get; set; }
    }
}
