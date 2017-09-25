using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Database;
using Localization.CoreLibrary.Entity;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
namespace Localization.CoreLibrary.Manager.Impl
{
    internal class DatabaseLocalizationManager : LocalizationManager, ILocalizationManager, IDatabaseDynamicTextService
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        
        private readonly IDatabaseTranslateService m_dbTranslateService;
        private readonly IDatabaseDynamicTextService m_databaseDynamicTextService;

        public DatabaseLocalizationManager(IConfiguration configuration, IDatabaseTranslateService dbTranslateService, IDatabaseDynamicTextService databaseDynamicTextService) : base(configuration)
        {
            m_dbTranslateService = dbTranslateService;
            m_databaseDynamicTextService = databaseDynamicTextService;

            Check();
        }

        //TODO: Check if default culture and supported cultures from json config file are in DB. If not, new are created.
        private void Check()
        {
            
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            cultureInfo = CultureInfoNullCheck(cultureInfo);
            scope = ScopeNullCheck(scope);

            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslate(text, cultureInfo, scope);
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


            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslateFormat(text, parameters, cultureInfo, scope);
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

            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslatePluralization(text, number, cultureInfo, scope);
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

            LocalizedString resultLocalizedString = m_dbTranslateService.DatabaseTranslateConstant(text, cultureInfo, scope);
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

        public DynamicText SaveDynamicText(DynamicText dynamicText)
        {
            return m_databaseDynamicTextService.SaveDynamicText(dynamicText);
        }
    }
}