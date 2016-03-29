using System.Globalization;
using System.Web.Mvc;

namespace Localization.Web
{
    class LocalizeController : Controller
    {

        public JsonResult Translation(string lang)
        {
            var dictionary = LocalizationManager.Instance.GetTranslation(new CultureInfo(lang));    //TODO add filtration of key:value pairs which client dont need by config
            return Json(dictionary);
        }
    }
}
