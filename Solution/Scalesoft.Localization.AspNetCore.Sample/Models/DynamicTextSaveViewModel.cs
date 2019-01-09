using System.ComponentModel.DataAnnotations;
using Scalesoft.Localization.Core.Model;

namespace Scalesoft.Localization.AspNetCore.Sample.Models
{
    public class DynamicTextSaveViewModel : DynamicTextViewModel
    {
        [Display(Name = "default-culture-action")]
        public IfDefaultNotExistAction IfDefaultNotExistAction { get; set; }
    }
}