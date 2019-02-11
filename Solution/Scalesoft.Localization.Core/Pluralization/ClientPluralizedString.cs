using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Common;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class ClientPluralizedString
    {
        public IList<ClientIntervalWithTranslation> Intervals;
        public LocalizedString DefaultLocalizedString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pluralizedString">Pluralized string to be converted</param>
        /// <exception cref="ArgumentNullException">If defaultLocalizedString is null.</exception>
        public ClientPluralizedString(PluralizedString pluralizedString)
        {
            Guard.ArgumentNotNull(nameof(pluralizedString), pluralizedString);

            DefaultLocalizedString = pluralizedString.GetDefaultLocalizedString();
            Intervals = pluralizedString.GetPluralizationDictionary().Select(x => new ClientIntervalWithTranslation{Interval = x.Key, LocalizedString = x.Value}).ToList();
        }
    }

    public class ClientIntervalWithTranslation
    {
        public PluralizationInterval Interval { get; set; }
        public LocalizedString LocalizedString { get; set; }
    }
}