using System;
using System.Globalization;
using Localization.CoreLibrary.Manager;
using Localization.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocalization m_fileLocalizationManager;

        public HomeController(ILocalization fileLocalizationManager)
        {
            m_fileLocalizationManager = fileLocalizationManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            RequestCulture requestCulture = new RequestCulture(culture);//TODO: Validation?

            HttpContext.Response.Cookies.Append(
                "Localization.Culture",
                requestCulture.Culture.Name,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
