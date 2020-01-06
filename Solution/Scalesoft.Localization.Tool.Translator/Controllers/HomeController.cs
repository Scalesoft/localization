using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Tool.Translator.Models;

namespace Scalesoft.Localization.Tool.Translator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> m_logger;

        public HomeController(ILogger<HomeController> logger)
        {
            m_logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
