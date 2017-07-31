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
        private DictionaryManager m_dictionaryManager;


        public LocalizationManager(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        public void AddDictionaryManager(IDictionaryManager dictionaryManager)
        {
            m_dictionaryManager = (DictionaryManager)dictionaryManager;
        }

        public LocalizedString Translate(string text, CultureInfo cultureInfo = null, string scope = null, string part = null)
        {
            string s;
            if (cultureInfo == null)
            {
                cultureInfo = m_configuration.DefaultCulture();
            }
            string scopestr = "";
            if (scope != null)
            {
                scopestr = String.Concat(DictionaryManager.ScopeDelimiter, scope);
            }

            if (part == null)
            {
                s = (string)m_dictionaryManager.Dictionaries[string.Concat(cultureInfo.Name, scopestr)].SelectToken("dictionary.parts.*."+text);
            }
            else
            {
                s = (string)m_dictionaryManager.Dictionaries[string.Concat(cultureInfo.Name,scopestr)][part][text];
            }
            if (s == null)
            {
                s = text;
            }
            
            return new LocalizedString(text, s);
        }

        public LocalizedString TranslateFormat(string text, string[] parameters, CultureInfo cultureInfo = null, string scope = null, string part = null)
        {
            throw new NotImplementedException();
        }
    }
}