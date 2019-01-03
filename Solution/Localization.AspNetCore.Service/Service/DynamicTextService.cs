using System.Collections.Generic;
using System.Globalization;
using Localization.AspNetCore.Service.Manager;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Manager;
using Localization.CoreLibrary.Model;

namespace Localization.AspNetCore.Service.Service
{
    public class DynamicTextService : ServiceBase, IDynamicText
    {
        private readonly IDatabaseDictionaryManager m_databaseDictionaryManager;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DynamicTextService(
            RequestCultureManager requestCultureManager,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IDatabaseDynamicTextService databaseDynamicTextService
        ) : base(requestCultureManager)
        {
            m_databaseDictionaryManager = databaseDictionaryManager;
            m_databaseDynamicTextService = databaseDynamicTextService;
        }

        public DynamicText GetDynamicText(string name, string scope)
        {
            var requestCulture = RequestCulture();

            return m_databaseDynamicTextService.GetDynamicText(name, scope, requestCulture);
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            return m_databaseDynamicTextService.GetAllDynamicText(name, scope);
        }

        public DynamicText SaveDynamicText(
            DynamicText dynamicText,
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
