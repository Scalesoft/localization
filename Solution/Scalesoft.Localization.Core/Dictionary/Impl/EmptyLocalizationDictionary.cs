using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Localization;
using Scalesoft.Localization.Core.Pluralization;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]

namespace Scalesoft.Localization.Core.Dictionary.Impl
{
    internal class EmptyLocalizationDictionary : ILocalizationDictionary
    {
        public const string EmptyExtension = ""; //Should be empty.

        public bool IsRoot => false;

        public IList<ILocalizationDictionary> ChildDictionaries { get; }

        public EmptyLocalizationDictionary()
        {
            ChildDictionaries = new List<ILocalizationDictionary>();
        }

        public EmptyLocalizationDictionary(Stream resourceStream) : this()
        {
        }

        public EmptyLocalizationDictionary(Stream resourceStream, string filePath) : this(resourceStream)
        {
        }

        public CultureInfo CultureInfo()
        {
            return System.Globalization.CultureInfo.InvariantCulture;
        }

        public string Scope()
        {
            return string.Empty;
        }

        public IList<string> ScopeAlias()
        {
            return new List<string>();
        }

        public string GetParentScopeName()
        {
            return string.Empty;
        }

        public string Extension()
        {
            return string.Empty;
        }

        public IDictionary<string, LocalizedString> List()
        {
            return new ConcurrentDictionary<string, LocalizedString>();
        }

        public IDictionary<string, PluralizedString> ListPlurals()
        {
            return new ConcurrentDictionary<string, PluralizedString>();
        }

        public IDictionary<string, LocalizedString> ListConstants()
        {
            return new ConcurrentDictionary<string, LocalizedString>();
        }

        public ILocalizationDictionary ParentDictionary()
        {
            return null;
        }

        public bool SetParentDictionary(ILocalizationDictionary parentDictionary)
        {
            return false;
        }

        public void SetChildDictionary(ILocalizationDictionary childDictionary)
        {
        }

        public bool IsLeaf()
        {
            return ChildDictionaries.Count == 0;
        }
    }
}