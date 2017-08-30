using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Localization.Database.Abstractions.Entity;

namespace Localization.Database.EFCore.Entity
{
    public abstract class BaseText : IBaseText
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }      

        [Required]
        [Column(TypeName = "smallint")]
        public int Format { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ModificationTime { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ModificationUser { get; set; }

        [Required]
        public DictionaryScope DictionaryScope { get; set; }

        public int CultureId { get; set; }
        public int DictionaryScopeId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Text { get; set; }

        [Required]
        public Culture Culture { get; set; }

        IDictionaryScope IBaseText.DictionaryScope { get; set; }

        ICulture IBaseText.Culture { get; set; }
    }
}