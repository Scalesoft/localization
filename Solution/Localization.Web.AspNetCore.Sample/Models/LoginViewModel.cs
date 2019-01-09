using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Scalesoft.Localization.AspNetCore.Sample.Models
{
    public class LoginViewModel
    {
        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
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
}