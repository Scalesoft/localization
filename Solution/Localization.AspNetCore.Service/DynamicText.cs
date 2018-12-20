using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Models;
using Microsoft.AspNetCore.Http;

namespace Localization.AspNetCore.Service
{
    public class DynamicText : ServiceBase, IDynamicText
    {
        private readonly IDatabaseDictionaryManager m_databaseDictionaryManager;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DynamicText(
            IHttpContextAccessor httpContextAccessor,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IDatabaseDynamicTextService databaseDynamicTextService
            )
            : base(httpContextAccessor)
        {
            m_databaseDictionaryManager = databaseDictionaryManager;
            m_databaseDynamicTextService = databaseDynamicTextService;
        }

        public CoreLibrary.Entity.DynamicText GetDynamicText(string name, string scope)
        {
            var requestCulture = RequestCulture();

            return m_databaseDynamicTextService.GetDynamicText(name, scope, requestCulture);
        }

        public IList<CoreLibrary.Entity.DynamicText> GetAllDynamicText(string name, string scope)
        {
            return m_databaseDynamicTextService.GetAllDynamicText(name, scope);
        }

        public CoreLibrary.Entity.DynamicText SaveDynamicText(
            CoreLibrary.Entity.DynamicText dynamicText,
            IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing
        )
        {
            return m_databaseDynamicTextService.SaveDynamicText(dynamicText, actionForDefaultCulture);
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            m_databaseDynamicTextService.DeleteAllDynamicText(name, scope);
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            m_databaseDynamicTextService.DeleteDynamicText(name, scope, cultureInfo);
        }

        private CultureInfo RequestCulture()
        {
            var request = HttpContextAccessor.HttpContext.Request;

            var cultureCookie = request.Cookies[CultureCookieName] ??
                                m_databaseDictionaryManager.DefaultCulture().Name;

            return new CultureInfo(cultureCookie);
        }
    }
}
