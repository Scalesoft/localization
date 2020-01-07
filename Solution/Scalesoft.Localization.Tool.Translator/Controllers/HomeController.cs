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
        private readonly EditorDataConverter m_editorDataConverter;
        private readonly ILogger<HomeController> m_logger;

        public HomeController(DictionaryManager dictionaryManager, EditorDataConverter editorDataConverter, ILogger<HomeController> logger)
        {
            m_dictionaryManager = dictionaryManager;
            m_editorDataConverter = editorDataConverter;
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
            var editorData = m_editorDataConverter.ConvertToViewModel(data);

            return PartialView("_Editor", editorData);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult NewCulture()
        {
            return View(new NewCultureViewModel());
        }

        public IActionResult CreateNewCulture(NewCultureViewModel data)
        {
            if (string.IsNullOrEmpty(data.CultureName))
            {
                return RedirectToAction("NewCulture");
            }

            m_dictionaryManager.CreateDictionariesForCulture(data.CultureName);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
