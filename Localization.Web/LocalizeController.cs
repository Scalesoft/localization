using System.Globalization;
using System.Web.Mvc;

namespace Localization.Web
{
    public class LocalizeController : Controller
    {
        public ActionResult Translation(string lang)
        {
            var dictionary = LocalizationManager.Instance.GetTranslation(new CultureInfo(lang));    //TODO add filtration of key:value pairs which client dont need by config
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }
    }
}
