using Microsoft.AspNetCore.Mvc;

namespace Scalesoft.Localization.AspNetCore.Sample.Controllers
{
    public class LocalizationController : Controller
    {
        private readonly IDictionaryService m_dictionary;

        public LocalizationController(IDictionaryService dictionary)
        {
            m_dictionary = dictionary;
        }

        [HttpGet]
        public IActionResult Dictionary(string scope)
        {
            return Json(m_dictionary.GetDictionary(scope));
        }

        [HttpGet]
        public IActionResult PluralizedDictionary(string scope)
        {
            return Json(m_dictionary.GetClientPluralizedDictionary(scope));
        }
    }
}