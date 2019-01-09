using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Scalesoft.Localization.AspNetCore.Sample.Models
{
    public class DynamicTextViewModel
    {
        public IEnumerable<SelectListItem> SupportedCultures { get; set; }

        [Display(Name = "culture")]
        [Required]
        public CultureInfo Culture { get; set; }

        [Display(Name = "name")]
        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }
        public string Scope { get; set; }
    }
}
