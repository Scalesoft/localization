using System.Collections.Generic;
using System.Globalization;
using Localization.AspNetCore.Service.Manager;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Models;

namespace Localization.AspNetCore.Service
{
    public class DynamicText : ServiceBase, IDynamicText
    {
        private readonly IDatabaseDictionaryManager m_databaseDictionaryManager;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DynamicText(
            RequestCultureManager requestCultureManager,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IDatabaseDynamicTextService databaseDynamicTextService
        ) : base(requestCultureManager)
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
            return GetRequestCulture(m_databaseDictionaryManager.DefaultCulture());
        }
    }
}
