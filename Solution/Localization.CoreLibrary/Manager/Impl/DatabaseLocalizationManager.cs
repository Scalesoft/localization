using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Entity;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Models;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
[assembly: InternalsVisibleTo("Localization.Database.EFCore.Tests")]

namespace Localization.CoreLibrary.Manager.Impl
{
    internal class DatabaseLocalizationManager : LocalizationManager, ILocalizationManager, IDatabaseDynamicTextService
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly IDatabaseTranslateService m_dbTranslateService;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DatabaseLocalizationManager(IConfiguration configuration, IDatabaseTranslateService dbTranslateService,
            IDatabaseDynamicTextService databaseDynamicTextService) : base(configuration)
        {
            m_dbTranslateService = dbTranslateService;
            m_databaseDynamicTextService = databaseDynamicTextService;

            Check();
        }

        private void Check()
        {
            m_dbTranslateService.CheckCulturesInDatabase();
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            var resultLocalizedString = m_dbTranslateService.DatabaseTranslate(text, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);


            var resultLocalizedString = m_dbTranslateService.DatabaseTranslateFormat(text, parameters, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            var resultLocalizedString = m_dbTranslateService.DatabaseTranslatePluralization(text, number, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            var resultLocalizedString = m_dbTranslateService.DatabaseTranslateConstant(text, cultureInfo, scope);
            if (resultLocalizedString == null)
            {
                resultLocalizedString = TranslateFallback(text);
            }

            return resultLocalizedString;
        }

        public CultureInfo DefaultCulture()
        {
            return Configuration.DefaultCulture();
        }

        public string DefaultScope()
        {
            return Localization.DefaultScope;
        }


        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            return m_databaseDynamicTextService.GetDynamicText(name, scope, cultureInfo);
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            return m_databaseDynamicTextService.GetAllDynamicText(name, scope);
        }

        public DynamicText SaveDynamicText(DynamicText dynamicText, IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing)
        {
            return m_databaseDynamicTextService.SaveDynamicText(dynamicText, actionForDefaultCulture);
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            m_databaseDynamicTextService.DeleteDynamicText(name, scope, cultureInfo);
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            m_databaseDynamicTextService.DeleteAllDynamicText(name, scope);
        }
    }
}
