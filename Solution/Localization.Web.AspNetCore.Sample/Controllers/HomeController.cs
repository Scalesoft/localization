using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Scalesoft.Localization.AspNetCore.Sample.Models;

namespace Scalesoft.Localization.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocalizationService m_localizationManager;

        public HomeController(ILocalizationService localizationManager, IDictionaryService dictionaryManager)
        {
            m_localizationManager = localizationManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact(LoginViewModel viewModel)
        {
            var usernameLabel = m_localizationManager.Translate("UserName", "LoginViewModel");
            var passwordLabel = m_localizationManager.Translate("Password");

            //return JsonConvert.SerializeObject(m_dictionaryManager.GetDictionary("home"), Formatting.Indented);
            //return Json(m_dictionaryManager.GetDictionary("home"));
            ViewData["username"] = usernameLabel;
            ViewData["password"] = passwordLabel;

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var requestCulture = new RequestCulture(culture); //TODO validation of unsupported culture name
            HttpContext.Request.HttpContext.Response.Cookies.Append(
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
