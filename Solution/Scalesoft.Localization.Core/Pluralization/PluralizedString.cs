using System;
using System.Collections.Concurrent;
using System.Linq;
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

        public ConcurrentBag<IntervalWithTranslation> Intervals { get; private set; }
        public LocalizedString DefaultLocalizedString { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defaultLocalizedString">Default Localized string. Used if requested number does not fit in any interval.</param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException">If defaultLocalizedString is null.</exception>
        public PluralizedString(LocalizedString defaultLocalizedString, ILogger logger = null)
        {
            Guard.ArgumentNotNull(nameof(defaultLocalizedString), defaultLocalizedString, logger);

            DefaultLocalizedString = defaultLocalizedString;
            m_logger = logger;
            Intervals = new ConcurrentBag<IntervalWithTranslation>();
        }

        /// <summary>
        /// Returns correct pluralized string.
        /// </summary>
        /// <param name="number">Plural number.</param>
        /// <returns>Pluralized string or default string if not found.</returns>
        public LocalizedString GetPluralizedLocalizedString(int number)
        {
            var pluralizationKey = new PluralizationInterval(number, number);

            foreach (var pluralizedLocalizedString in Intervals)
            {
                if (pluralizedLocalizedString.Interval.Equals(pluralizationKey))
                {
                    return pluralizedLocalizedString.LocalizedString;
                }
            }

            return DefaultLocalizedString;
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
            Intervals.Add(new IntervalWithTranslation{Interval = pluralizationInterval , LocalizedString = localizedString});
        }

        /// <summary>
        /// Checks if given pluralizationInterval overlaps with already added intervals.
        /// </summary>
        /// <param name="pluralizationInterval">Interval to check.</param>
        /// <returns>True if overlap was found.</returns>
        private bool CheckOverlaping(PluralizationInterval pluralizationInterval)
        {
            Guard.ArgumentNotNull(nameof(pluralizationInterval), pluralizationInterval, m_logger);

            var pluralizedIntervals = Intervals.Select(x => x.Interval);
            foreach (var pluralizedInterval in pluralizedIntervals)
            {
                if (pluralizedInterval.IsOverlaping(pluralizationInterval))
                {
                    return true;
                }
            }

            return false;
        }
    }
}