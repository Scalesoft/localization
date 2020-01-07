using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Tool.Translator.Core;
using Scalesoft.Localization.Tool.Translator.Models;

namespace Scalesoft.Localization.Tool.Translator.Controllers
{
    public class HomeController : Controller
    {
        private readonly DictionaryManager m_dictionaryManager;
        private readonly ILogger<HomeController> m_logger;

        public HomeController(DictionaryManager dictionaryManager, ILogger<HomeController> logger)
        {
            m_dictionaryManager = dictionaryManager;
            m_logger = logger;
        }

        public IActionResult Index()
        {
            var data = m_dictionaryManager.GetData();

            return View(data);
        }

        public IActionResult GetEditor(string scope)
        {
            var data = m_dictionaryManager.GetDataForScope(scope);

            return PartialView("_Editor", data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
