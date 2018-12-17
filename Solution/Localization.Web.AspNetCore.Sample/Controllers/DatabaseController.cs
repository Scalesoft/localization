using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Localization.AspNetCore.Service;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Util;
using Localization.Web.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DynamicTextService = Localization.AspNetCore.Service.DynamicText;
using DynamicTextEntity = Localization.CoreLibrary.Entity.DynamicText;

namespace Localization.Web.AspNetCore.Sample.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly ILocalization m_localization;
        private readonly DynamicTextService m_dynamicTextService;

        public DatabaseController(ILocalization localization, DynamicTextService dynamicTextService)
        {
            m_localization = localization;
            m_dynamicTextService = dynamicTextService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDynamicText()
        {
            var model = new DynamicTextSaveViewModel
            {
                SupportedCultures = GetSupportedCultures()
            };

            return View(model);
        }

        private IEnumerable<SelectListItem> GetSupportedCultures()
        {
            var result = m_localization.SupportedCultures().Select(i =>
                new SelectListItem()
                {
                    Text = i.DisplayName,
                    Value = i.Name
                });
            return result;
        }

        public IActionResult SaveDynamicText(DynamicTextSaveViewModel dynamicText)
        {
            if (ModelState.IsValid)
            {
                m_dynamicTextService.SaveDynamicText(new DynamicTextEntity
                {
                    Culture = dynamicText.Culture.Name,
                    DictionaryScope = dynamicText.Scope ?? "global",
                    FallBack = false,
                    Text = dynamicText.Text,
                    Name = dynamicText.Name
                }, dynamicText.DefaultCultureAction);
            }

            return View("Index");
        }

        public IActionResult GetDynamicText(string name, string scope)
        {
            var model = m_dynamicTextService.GetDynamicText(name, scope);

            var directTranslation = m_localization.Translate(name, scope, LocTranslationSource.Database);

            var allDynamicTexts = m_dynamicTextService.GetAllDynamicText(name, scope);

            var result = new DynamicTextResult
            {
                Name = model.Name,
                Text = model.Text,
                DirectTranslation = directTranslation,
                AllDynamicTexts = allDynamicTexts.Select(x => new CultureAndTextResult
                {
                    Culture = x.Culture,
                    Text = x.Text,
                }).ToList(),
            };
            return View("DynamicTextResult", result);
        }

        [HttpGet]
        public IActionResult DeleteAllDynamicText()
        {
            return View("DeleteAllDynamicText");
        }

        [HttpPost]
        public IActionResult DeleteAllDynamicText(string name, string scope)
        {
            m_dynamicTextService.DeleteAllDynamicText(name, scope);

            return View("Index");
        }

        [HttpGet]
        public IActionResult DeleteDynamicText()
        {
            var model = new DynamicTextViewModel
            {
                SupportedCultures = GetSupportedCultures()
            };

            return View("DeleteDynamicText", model);
        }

        [HttpPost]
        public IActionResult DeleteDynamicText(DynamicTextViewModel dynamicTextViewModel)
        {
            m_dynamicTextService.DeleteDynamicText(dynamicTextViewModel.Name, dynamicTextViewModel.Scope, dynamicTextViewModel.Culture);

            return View("Index");
        }
    }
}