using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Localization.Web.AspNetCore.Sample.Models
{
    public class DynamicTextViewModel
    {
        public IEnumerable<SelectListItem> SupportedCultures { get; set; }

        [Display(Name = "culture")]
        public CultureInfo Culture { get; set; }

        [Display(Name = "name")]
        public string Name { get; set; }

        public string Text { get; set; }
        public string Scope { get; set; }
    }
}
