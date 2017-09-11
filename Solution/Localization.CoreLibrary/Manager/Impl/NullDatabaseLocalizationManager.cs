using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
namespace Localization.CoreLibrary.Manager.Impl
{
    internal class NullDatabaseLocalizationManager : ILocalizationManager
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private const string NotSetMessage = "Database localization manager is not set.";

        private void LogNotSet()
        {
            if (Logger.IsInformationEnabled())
            {
                Logger.LogInformation(NotSetMessage);
            }
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();
            return null;
        }

        public LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null,
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