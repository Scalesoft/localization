using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Localization.Database.EFCore.Entity
{
    public sealed class DatabaseVersion
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Version { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string SolutionVersion { get; set; }

        [Required]
        public DateTime UpgradeDate { get; set; }

        [Required] //TODO SEE SQL GEN. SCHEMA
        [Column(TypeName = "varchar(150)")]
        public string UpgradeUser { get; set; }

    }
}