using System.Collections.Generic;
using System.Globalization;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Model;

namespace Scalesoft.Localization.AspNetCore.Service
{
    public class DynamicTextService : ServiceBase, IDynamicTextService
    {
        private readonly IDatabaseDictionaryManager m_databaseDictionaryManager;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DynamicTextService(
            IRequestCultureManager requestCultureManager,
            IDatabaseDictionaryManager databaseDictionaryManager,
            IDatabaseDynamicTextService databaseDynamicTextService
        ) : base(requestCultureManager)
        {
            m_databaseDictionaryManager = databaseDictionaryManager;
            m_databaseDynamicTextService = databaseDynamicTextService;
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo culture = null)
        {
            culture = culture ?? RequestCulture();

            return m_databaseDynamicTextService.GetDynamicText(name, scope, culture);
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
            return GetRequestCulture(m_databaseDictionaryManager.GetDefaultCulture());
        }
    }
}
