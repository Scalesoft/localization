using System.Globalization;
using System.Web.Mvc;
using Localization.Core;

namespace Localization.Web.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangeTranslation(string locale)
        {
            LocalizationManager.Instance.SetCultureInfo(new CultureInfo(locale));
            return RedirectToAction("Index","Home");
        }
    }
}