using System;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Localization.CoreLibrary.Exception;

namespace Localization.CoreLibrary.Pluralization
{
    public class PluralizedString
    {        
        private readonly Dictionary<PluralizationInterval, LocalizedString> m_pluralized;
        private readonly LocalizedString m_defaultLocalizedString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="defaultLocalizedString">Default Localized string. Used if requested number does not fit in any interval.</param>
        public PluralizedString(LocalizedString defaultLocalizedString)
        {
            if (defaultLocalizedString == null)
            {
                throw new PluralizedDefaultStringException("The defaultLocalizedString cannot be null.");
            }

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
                throw new PluralizedStringIntervalOverlapException("Intervals are overlaping in the Pluralized string.");
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
            if (pluralizationInterval == null)
            {
                throw new ArgumentNullException(nameof(pluralizationInterval));
            }

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