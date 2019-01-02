using System;
using Localization.AspNetCore.Service;
using Localization.CoreLibrary.Logging;
using Localization.Web.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocalization m_localizationManager;
        private readonly IDictionary m_dictionaryManager;
        private static readonly ILogger m_logger = LogProvider.GetCurrentClassLogger();

        public HomeController(ILocalization localizationManager, IDictionary dictionaryManager)
        {
            m_localizationManager = localizationManager;
            m_dictionaryManager = dictionaryManager;
        }

        public IActionResult Index()
        {
            //m_staticTextsContext.Culture.Add(new Culture() { Name = "en-US" });
            //m_staticTextsContext.DictionaryScope.Add(new DictionaryScope() { Name = "global" });

            //DictionaryScope dictionaryScope = m_staticTextsContext.DictionaryScope.Single(ds => ds.Name == "global");
            //Culture culture = m_staticTextsContext.Culture.Single(c => c.Name == "en-US");
            //StaticTextBuilder stb = new StaticTextBuilder();
            //stb.DictionaryScope(dictionaryScope)
            //    .Culture(culture)
            //    .Format(0)
            //    .Name("name-key")
            //    .Text("englidh text")
            //    .ModificationUser("Jiri");

            //m_staticTextsContext.StaticText.Add(stb.Build());
            //m_staticTextsContext.SaveChanges();

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