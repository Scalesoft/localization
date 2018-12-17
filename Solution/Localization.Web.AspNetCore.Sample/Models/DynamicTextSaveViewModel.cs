using System.ComponentModel.DataAnnotations;
using Localization.CoreLibrary.Models;

namespace Localization.Web.AspNetCore.Sample.Models
{
    public class DynamicTextSaveViewModel: DynamicTextViewModel
    {
        [Display(Name = "default-culture-action")]
        public DefaultCultureAction DefaultCultureAction { get; set; }
    }
}
