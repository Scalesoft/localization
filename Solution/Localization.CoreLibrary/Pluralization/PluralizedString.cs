using System;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Localization.CoreLibrary.Common;
using Localization.CoreLibrary.Exception;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Pluralization
{
    public class PluralizedString
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly Dictionary<PluralizationInterval, LocalizedString> m_pluralized;
        private readonly LocalizedString m_defaultLocalizedString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defaultLocalizedString">Default Localized string. Used if requested number does not fit in any interval.</param>
        /// <exception cref="ArgumentNullException">If defaultLocalizedString is null.</exception>
        public PluralizedString(LocalizedString defaultLocalizedString)
        {
            Guard.ArgumentNull(nameof(defaultLocalizedString), defaultLocalizedString, Logger);

            m_defaultLocalizedString = defaultLocalizedString; 
            m_pluralized = new Dictionary<PluralizationInterval, LocalizedString>();
        }

        /// <summary>
        /// Returns correct pluralized string.
        /// </summary>
        /// <param name="number">Plural number.</param>
        /// <returns>Pluralized string or default string if not found.</returns>
        public LocalizedString GetPluralizedLocalizedString(int number)
        {
            PluralizationInterval pluralizationKey = new PluralizationInterval(number, number);

            foreach (KeyValuePair<PluralizationInterval, LocalizedString> pluralizedLocalizedString in m_pluralized)
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
                string overlapErrorMsg = "Intervals are overlaping in the Pluralized string.";
                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(overlapErrorMsg);
                }

                throw new PluralizedStringIntervalOverlapException(overlapErrorMsg);
            }        

            m_pluralized.Add(pluralizationInterval, localizedString);
        }

        /// <summary>
        /// Checks if given pluralizationInterval overlaps with already added intervals.
        /// </summary>
        /// <param name="pluralizationInterval">Interval to check.</param>
        /// <returns>True if overlap was found.</returns>
        private bool CheckOverlaping(PluralizationInterval pluralizationInterval)
        {
            Guard.ArgumentNull(nameof(pluralizationInterval), pluralizationInterval, Logger);

            Dictionary<PluralizationInterval, LocalizedString>.KeyCollection pluralizedKeys = m_pluralized.Keys;
            foreach (PluralizationInterval pluralizedKey in pluralizedKeys)
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