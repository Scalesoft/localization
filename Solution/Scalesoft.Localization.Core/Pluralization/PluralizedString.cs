using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Common;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Logging;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class PluralizedString
    {
        private readonly ILogger m_logger;

        private readonly ConcurrentDictionary<PluralizationInterval, LocalizedString> m_pluralized;
        private readonly LocalizedString m_defaultLocalizedString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defaultLocalizedString">Default Localized string. Used if requested number does not fit in any interval.</param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException">If defaultLocalizedString is null.</exception>
        public PluralizedString(LocalizedString defaultLocalizedString, ILogger logger = null)
        {
            Guard.ArgumentNotNull(nameof(defaultLocalizedString), defaultLocalizedString, logger);

            m_defaultLocalizedString = defaultLocalizedString;
            m_logger = logger;
            m_pluralized = new ConcurrentDictionary<PluralizationInterval, LocalizedString>();
        }

        /// <summary>
        /// Returns correct pluralized string.
        /// </summary>
        /// <param name="number">Plural number.</param>
        /// <returns>Pluralized string or default string if not found.</returns>
        public LocalizedString GetPluralizedLocalizedString(int number)
        {
            var pluralizationKey = new PluralizationInterval(number, number);

            foreach (var pluralizedLocalizedString in m_pluralized)
            {
                if (pluralizedLocalizedString.Key.Equals(pluralizationKey))
                {
                    return pluralizedLocalizedString.Value;
                }
            }

            return m_defaultLocalizedString;
        }

        /// <summary>
        /// Adds new pluralized form of Localized string with its interval.
        /// </summary>
        /// <param name="pluralizationInterval">Interval when use this form of localized string.</param>
        /// <param name="localizedString">Assigned pluralized form.</param>
        /// <exception cref="PluralizedStringIntervalOverlapException">Thrown if Pluralized string already contains any 
        /// sub-interval of paramater pluralizationInterval</exception>
        public void Add(PluralizationInterval pluralizationInterval, LocalizedString localizedString)
        {
            if (CheckOverlaping(pluralizationInterval))
            {
                var overlapErrorMsg = "Intervals are overlaping in the Pluralized string.";
                if (m_logger != null && m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(overlapErrorMsg);
                }

                throw new PluralizedStringIntervalOverlapException(overlapErrorMsg);
            }

            m_pluralized.TryAdd(pluralizationInterval, localizedString);
        }

        /// <summary>
        /// Checks if given pluralizationInterval overlaps with already added intervals.
        /// </summary>
        /// <param name="pluralizationInterval">Interval to check.</param>
        /// <returns>True if overlap was found.</returns>
        private bool CheckOverlaping(PluralizationInterval pluralizationInterval)
        {
            Guard.ArgumentNotNull(nameof(pluralizationInterval), pluralizationInterval, m_logger);

            var pluralizedKeys = m_pluralized.Keys;
            foreach (var pluralizedKey in pluralizedKeys)
            {
                if (pluralizedKey.IsOverlaping(pluralizationInterval))
                {
                    return true;
                }
            }

            return false;
        }
    }
}