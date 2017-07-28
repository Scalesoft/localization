using System;
using System.Globalization;
using Localization.CoreLibrary.Util;
using Microsoft.Extensions.Localization;

namespace Localization.CoreLibrary.Manager.Impl
{
    //TODO
    public class LocalizationManager : ILocalizationManager
    {
        private IConfiguration m_configuration;


        public LocalizationManager(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null, string part = null)
        {
            throw new NotImplementedException();
        }

        public LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null, string part = null)
        {
            throw new NotImplementedException();
        }
    }
}