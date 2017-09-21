using Localization.Web.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class LoginController : Microsoft.AspNetCore.Mvc.Controller
    {
        public IActionResult About(LoginViewModel model, string returnUrl = null)
        {

            return View(model);
        }
    }
}