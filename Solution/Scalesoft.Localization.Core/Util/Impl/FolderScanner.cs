using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary;

[assembly: InternalsVisibleTo("Scalesoft.Localization.Core.Tests")]

namespace Scalesoft.Localization.Core.Util.Impl
{
    internal class FolderScanner : ILocalizationStructureScanner
    {
        private readonly IDictionaryFactory m_dictionaryFactory;
        private readonly LocalizationConfiguration m_configuration;

        public FolderScanner(IDictionaryFactory dictionaryFactory, LocalizationConfiguration configuration)
        {
            m_dictionaryFactory = dictionaryFactory;
            m_configuration = configuration;
        }

        /// <summary>
        /// Scans recursively folders inside basePath.
        /// </summary>
        /// <param name="basePath">FileLocalization base path.</param>
        /// <returns>Collection containing full names of all dictionary files.</returns>
        public IList<string> GetAllDictionaryFullpaths(string basePath)
        {
            var listOfFiles = new List<string>();

            if (m_configuration.IsContainerEnvironment)
            {
                GetFiles(basePath, listOfFiles);
            }
            else
            {
                var allFiles = Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories).Where(x => m_dictionaryFactory.FileExtensions.Any(x.EndsWith)).ToList();
                listOfFiles.AddRange(allFiles);
            }

            return listOfFiles;
        }

        private static void GetFiles(string dir, List<string> listOfFiles)
        {
            foreach (var dirPath in Directory.GetDirectories(dir))
            {
                var files = Directory.GetFiles(dirPath);

                if (files.Length == 0)
                {
                    GetFiles(dirPath, listOfFiles);
                }

                listOfFiles.AddRange(files);
            }
        }
    }
}
