using System;
using System.Threading.Tasks;
using Localization.Web.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User?.Identity.IsAuthenticated == true) return Redirect("~/");

            if ( Url.IsLocalUrl(returnUrl) || string.IsNullOrEmpty(returnUrl))
            {
                var viewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                }; ;

                return View(viewModel);
            }

            return BadRequest("Invalid return url");
        }
    }
}