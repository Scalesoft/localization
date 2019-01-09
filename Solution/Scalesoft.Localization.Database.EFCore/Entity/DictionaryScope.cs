using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.EFCore.Entity
{
    public sealed class DictionaryScope : IDictionaryScope
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }


        public ICollection<BaseText> BaseTexts { get; set; }
    }
}