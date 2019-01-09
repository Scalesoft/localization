using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.AspNetCore.Manager;
using Scalesoft.Localization.Core.Manager;
using Scalesoft.Localization.Core.Pluralization;
using Scalesoft.Localization.Core.Util;

namespace Scalesoft.Localization.AspNetCore.Service
{
    public class DictionaryService : ServiceBase, IDictionaryService
    {
        private readonly IAutoDictionaryManager m_dictionaryManager;

        public DictionaryService(
            RequestCultureManager requestCultureManager,
            IAutoDictionaryManager autoDictionaryManager
        ) : base(requestCultureManager)
        {
            m_dictionaryManager = autoDictionaryManager;
        }

        public IDictionary<string, LocalizedString> GetDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            var requestCulture = RequestCulture();

            return m_dictionaryManager.GetDictionary(translationSource, requestCulture, scope);
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            var requestCulture = RequestCulture();

            return m_dictionaryManager.GetPluralizedDictionary(translationSource, requestCulture, scope);
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(string scope = null,
            LocTranslationSource translationSource = LocTranslationSource.Auto)
        {
            var requestCulture = RequestCulture();

            return m_dictionaryManager.GetConstantsDictionary(translationSource, requestCulture, scope);
        }

        /// <summary>
        /// Gets and returns Culture from request httpContext culture cookie.
        /// </summary>
        /// <returns> Culture from request httpContext culture cookie.</returns>
        private CultureInfo RequestCulture()
        {
            return GetRequestCulture(m_dictionaryManager.GetDefaultCulture());
        }
    }
}
