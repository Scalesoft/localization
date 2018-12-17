using System.ComponentModel.DataAnnotations;

namespace Localization.CoreLibrary.Models
{
    public enum DefaultCultureAction
    {
        [Display(Name = "nothing")]
        Nothing,

        [Display(Name = "empty-string")]
        CreateEmpty,

        [Display(Name = "copy")]
        Copy
    }
}