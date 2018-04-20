using System;
using System.Globalization;
using Localization.AspNetCore.Service;
using Localization.AspNetCore.Service.Factory;
using Localization.CoreLibrary.Util;
using Localization.Web.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;


namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class HomeController : LoginController
    {
        private readonly ILocalization m_localizationManager;
        private readonly IDictionary m_dictionaryManager;

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        //[RequireHttps]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult Contact()
        {
            var translated = CoreLibrary.Localization.Translator.Translate(LocTranslationSource.Auto, "UserName");
            var passtr = CoreLibrary.Localization.Translator.Translate(LocTranslationSource.Auto, "Password");

            //return JsonConvert.SerializeObject(m_dictionaryManager.GetDictionary("home"), Formatting.Indented);
            //return Json(m_dictionaryManager.GetDictionary("home"));
            ViewData["username"] = translated;
            ViewData["password"] = passtr;
            return View(new LoginViewModel {UserName = translated, Password = passtr});
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            RequestCulture requestCulture = new RequestCulture(culture); //TODO: Validation?
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