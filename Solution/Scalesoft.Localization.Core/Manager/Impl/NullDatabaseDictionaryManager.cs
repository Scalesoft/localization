﻿using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Logging;
using Scalesoft.Localization.Core.Pluralization;

namespace Scalesoft.Localization.Core.Manager.Impl
{
    public class NullDatabaseDictionaryManager : IDatabaseDictionaryManager
    {
        private readonly ILogger m_logger;
        private const string NotSetMessage = "Database dictionary manager is not set.";

        public NullDatabaseDictionaryManager(ILogger<NullDatabaseDictionaryManager> logger = null)
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

        public IDictionary<string, LocalizedString> GetDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return new Dictionary<string, LocalizedString>();
        }

        public IDictionary<string, PluralizedString> GetPluralizedDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return new Dictionary<string, PluralizedString>();
        }

        public IDictionary<string, LocalizedString> GetConstantsDictionary(CultureInfo cultureInfo = null, string scope = null)
        {
            LogNotSet();

            return new Dictionary<string, LocalizedString>();
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
