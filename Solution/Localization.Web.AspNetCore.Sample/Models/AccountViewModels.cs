using System.ComponentModel.DataAnnotations;

namespace Localization.Web.AspNetCore.Sample.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserNameNotEmpty")]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "PasswordNotEmpty")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")] public bool RememberMe { get; set; }
    }
}