using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Common;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class ClientPluralizedString
    {
        public readonly ConcurrentDictionary<string, LocalizedString> Pluralized;
        public readonly LocalizedString DefaultLocalizedString;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pluralizedString">Pluralized string to be converted</param>
        /// <exception cref="ArgumentNullException">If defaultLocalizedString is null.</exception>
        public ClientPluralizedString(PluralizedString pluralizedString)
        {
            Guard.ArgumentNotNull(nameof(pluralizedString), pluralizedString);

            DefaultLocalizedString = pluralizedString.GetDefaultLocalizedString();
            var mappedDictionary = pluralizedString.GetPluralizationDictionary().ToDictionary(p => $"{p.Key.Start},{p.Key.End}", p => p.Value);
            Pluralized = new ConcurrentDictionary<string, LocalizedString>(mappedDictionary);
        }
    }
}