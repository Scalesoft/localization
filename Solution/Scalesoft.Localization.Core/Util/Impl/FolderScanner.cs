using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Scalesoft.Localization.Core.Dictionary;

[assembly: InternalsVisibleTo("Scalesoft.Localization.Core.Tests")]

namespace Scalesoft.Localization.Core.Util.Impl
{
    internal class FolderScanner : ILocalizationStructureScanner
    {
        private readonly IDictionaryFactory m_dictionaryFactory;

        public FolderScanner(IDictionaryFactory dictionaryFactory)
        {
            m_dictionaryFactory = dictionaryFactory;
        }

        /// <summary>
        /// Scans recursively folders inside basePath.
        /// </summary>
        /// <param name="basePath">FileLocalization base path.</param>
        /// <returns>Collection containing full names of all dictionary files.</returns>
        public IList<string> GetAllDictionaryFullpaths(string basePath)
        {
            return Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories)
                .Where(x => m_dictionaryFactory.FileExtensions.Any(x.EndsWith)).ToList();
        }
    }
}
