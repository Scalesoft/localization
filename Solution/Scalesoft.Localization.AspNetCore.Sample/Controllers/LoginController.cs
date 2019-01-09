using Microsoft.AspNetCore.Mvc;
using Scalesoft.Localization.AspNetCore.Sample.Models;

namespace Scalesoft.Localization.AspNetCore.Sample.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User?.Identity.IsAuthenticated == true) return Redirect("~/");

            if ( Url.IsLocalUrl(returnUrl) || string.IsNullOrEmpty(returnUrl))
            {
                var viewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                };

                return View(viewModel);
            }

            return BadRequest("Invalid return url");
        }
    }
}