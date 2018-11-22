using System.Collections.Generic;
using System.Globalization;
using Localization.CoreLibrary.Database;
using Microsoft.AspNetCore.Http;

namespace Localization.AspNetCore.Service
{
    public class DynamicText : ServiceBase, IDynamicText
    {
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DynamicText(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            m_databaseDynamicTextService = Localization.CoreLibrary.Localization.DynamicText;
        }

        public CoreLibrary.Entity.DynamicText GetDynamicText(string name, string scope)
        {
            CultureInfo requestCulture = RequestCulture();

            return m_databaseDynamicTextService.GetDynamicText(name, scope, requestCulture);
        }

        public IList<CoreLibrary.Entity.DynamicText> GetAllDynamicText(string name, string scope)
        {
            return m_databaseDynamicTextService.GetAllDynamicText(name, scope);
        }

        public CoreLibrary.Entity.DynamicText SaveDynamicText(CoreLibrary.Entity.DynamicText dynamicText)
        {
            return m_databaseDynamicTextService.SaveDynamicText(dynamicText);
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
            HttpRequest request = HttpContextAccessor.HttpContext.Request;

            string cultureCookie = request.Cookies[ServiceBase.CultureCookieName];
            if (cultureCookie == null)
            {
                cultureCookie = Localization.CoreLibrary.Localization.DatabaseDictionary.DefaultCulture().Name;
            }

            return new CultureInfo(cultureCookie);
        }
    }
}