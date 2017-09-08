using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Localization.CoreLibrary.Pluralization;
using Microsoft.Extensions.Localization;

[assembly: InternalsVisibleTo("Localization.CoreLibrary.Tests")]
namespace Localization.CoreLibrary.Dictionary.Impl
{
    internal class EmptyLocalizationDictionary : ILocalizationDictionary
    {
        public const string EmptyExtension = ""; //Should be empty.

        public ILocalizationDictionary Load(string filePath)
        {
            return this;
        }

        public CultureInfo CultureInfo()
        {
            return System.Globalization.CultureInfo.InvariantCulture;
        }

        public string Scope()
        {
            return string.Empty;
        }

        public string Extension()
        {
            return string.Empty;
        }

        public Dictionary<string, LocalizedString> List()
        {
            return new Dictionary<string, LocalizedString>();
        }

        public Dictionary<string, PluralizedString> ListPlurals()
        {
            return new Dictionary<string, PluralizedString>();
        }

        public Dictionary<string, LocalizedString> ListConstants()
        {
            return new Dictionary<string, LocalizedString>();
        }

        public ILocalizationDictionary ParentDictionary()
        {
            return null;
        }

        public ILocalizationDictionary ChildDictionary()
        {
            return null;
        }

        public bool SetParentDictionary(ILocalizationDictionary parentDictionary)
        {
            return false;
        }

        public bool SetChildDictionary(ILocalizationDictionary childDictionary)
        {
            return false;
        }

        public bool IsLeaf()
        {
            return true;
        }
    }
}