using Microsoft.AspNetCore.Mvc;

namespace Scalesoft.Localization.AspNetCore.Sample.Controllers
{
    public class LocalizationController : Controller
    {
        private readonly IDictionaryService m_dictionary;
        private readonly ILocalizationService m_localizationService;

        public LocalizationController(IDictionaryService dictionary, ILocalizationService localizationService)
        {
            m_dictionary = dictionary;
            m_localizationService = localizationService;
        }

        [HttpGet]
        public IActionResult Dictionary(string scope)
        {
            return Json(m_dictionary.GetDictionary(scope));
        }

        [HttpGet]
        public IActionResult PluralizedDictionary(string scope)
        {
            return Json(m_dictionary.GetPluralizedDictionary(scope));
        }

        [HttpGet]
        public IActionResult CurrentCulture()
        {
            return Content(m_localizationService.GetRequestCulture().Name);
        }
    }
}