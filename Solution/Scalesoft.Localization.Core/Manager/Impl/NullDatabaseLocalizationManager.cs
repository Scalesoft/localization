using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Logging;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class NullDatabaseLocalizationManager : IDatabaseLocalizationManager
    {
        private readonly ILogger m_logger;
        private const string NotSetMessage = "Database localization manager is not set.";

        public NullDatabaseLocalizationManager(ILogger<NullDatabaseLocalizationManager> logger = null)
        {
            m_logger = logger;
        }

        private void LogNotSet()
        {
            if (m_logger != null && m_logger.IsInformationEnabled())
            {
                m_logger.LogInformation(NotSetMessage);
            }
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return null;
        }

        public LocalizedString TranslateFormat(string text, object[] parameters, CultureInfo cultureInfo = null,
            string scope = null)
        {
            LogNotSet();

            return null;
        }

        public LocalizedString TranslatePluralization(string text, int number, CultureInfo cultureInfo = null,
            string scope = null)
        {
            LogNotSet();

            return null;
        }

        public LocalizedString TranslateConstant(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return null;
        }

        public CultureInfo DefaultCulture()
        {
            LogNotSet();

            return null;
        }

        public string DefaultScope()
        {
            LogNotSet();

            return null;
        }
    }
}
