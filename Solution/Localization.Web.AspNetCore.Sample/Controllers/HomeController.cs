using System;
using System.Linq;
using Localization.Database.EFCore.Data;
using Localization.Database.EFCore.Data.Impl;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.EntityBuilder;
using Localization.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILocalization m_fileLocalizationManager;
        private readonly StaticTextsContext m_staticTextsContext;

        public HomeController(ILocalization fileLocalizationManager, StaticTextsContext staticTextsContext)
        {
            m_fileLocalizationManager = fileLocalizationManager;
            m_staticTextsContext = staticTextsContext;
        }

        public IActionResult Index()
        {
            //m_staticTextsContext.Culture.Add(new Culture() { Name = "en-US" });
            //m_staticTextsContext.DictionaryScope.Add(new DictionaryScope() { Name = "global" });

            DictionaryScope dictionaryScope = m_staticTextsContext.DictionaryScope.Single(ds => ds.Name == "global");
            Culture culture = m_staticTextsContext.Culture.Single(c => c.Name == "en-US");
            StaticTextBuilder stb = new StaticTextBuilder();
            stb.DictionaryScope(dictionaryScope)
                .Culture(culture)
                .Format(0)
                .Name("name-key")
                .Text("englidh text")
                .ModificationUser("Jiri");

            m_staticTextsContext.StaticText.Add(stb.Build());
            m_staticTextsContext.SaveChanges();

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
