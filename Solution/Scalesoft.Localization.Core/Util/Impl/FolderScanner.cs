using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Dictionary;
using Scalesoft.Localization.Core.Exception;
using Scalesoft.Localization.Core.Logging;

[assembly: InternalsVisibleTo("Scalesoft.Localization.Core.Tests")]

namespace Scalesoft.Localization.Core.Util.Impl
{
    internal class FolderScanner : ILocalizationStructureScanner
    {
        private readonly ILogger m_logger;
        private readonly IDictionaryFactory m_dictionaryFactory;

        public FolderScanner(IDictionaryFactory dictionaryFactory, ILogger<FolderScanner> logger = null)
        {
            m_dictionaryFactory = dictionaryFactory;
            m_logger = logger;
        }

        /// <summary>
        /// Scans recursively folders inside basePath.
        /// </summary>
        /// <param name="basePath">FileLocalization base path.</param>
        /// <returns>array containing all scope folders.</returns>
        private static string[] ScanScopeDirectories(string basePath)
        {
            return Directory.GetDirectories(basePath, "*", SearchOption.AllDirectories);
        }

        private IEnumerable<string> CheckGlobalResourceFiles(LocalizationConfiguration libConfiguration)
        {
            IList<string> localizationFiles = new List<string>();
            var exceptionLogStringBuilder = new StringBuilder();
            var shouldThrowException = false;

            var defaultCulture = libConfiguration.DefaultCulture;

            var allSupportedCultures = libConfiguration.SupportedCultures.ToList();
            if (!allSupportedCultures.Contains(defaultCulture))
            {
                allSupportedCultures.Add(defaultCulture);
            }

            foreach (var supportedCulture in allSupportedCultures)
            {
                var supportedFilePathWithoutExtension = Path.Combine(libConfiguration.BasePath, supportedCulture.Name);

                var supportedCultureResourceFileFound = false;

                foreach (var fileExtension in m_dictionaryFactory.FileExtensions)
                {
                    var supportedCultureFilePath = string.Concat(supportedFilePathWithoutExtension, ".", fileExtension);

                    if (File.Exists(supportedCultureFilePath))
                    {
                        supportedCultureResourceFileFound = true;
                        localizationFiles.Add(supportedCultureFilePath);

                        break;
                    }
                }

                if (!supportedCultureResourceFileFound)
                {
                    var message = string.Format(
                        @"Global dictionary file ""{0}.[{1}]"" is missing. This may lead to library initialization failure if it is not provided otherwise.",
                        supportedFilePathWithoutExtension,
                        string.Join("|", m_dictionaryFactory.FileExtensions)
                    );
                    if (m_logger != null && m_logger.IsWarningEnabled())
                    {
                        m_logger.LogWarning(message);
                    }

                    /* global scope must be present in a culture even if a culture is dynamic user-created
                    it is required to build hierarchy
                    global dictionary however may be loaded manually*/
                    //exceptionLogStringBuilder.AppendLine(message);
                    //shouldThrowException = true;
                }
            }

            if (shouldThrowException)
            {
                throw new DictionaryLoadException(string.Concat("Missing resource file/s.", '\n',
                    exceptionLogStringBuilder.ToString()));
            }

            return localizationFiles;
        }

        /// <summary>
        /// Check for resource files in provided folders.
        /// Each folder represents scope. Scopes can be nested "infinitely".
        /// </summary>
        /// <param name="libConfiguration">Library configuration.</param>
        /// <param name="scopeDirectories">Array with all folders inside localization folder</param>
        private IEnumerable<string> CheckScopeResourceFiles(LocalizationConfiguration libConfiguration,
            string[] scopeDirectories)
        {
            IList<string> localizationFiles = new List<string>();

            var exceptionLogStringBuilder = new StringBuilder();
            var shouldThrowException = false;

            var allSupportedCultures = libConfiguration.SupportedCultures.ToList();
            if (!allSupportedCultures.Contains(libConfiguration.DefaultCulture))
            {
                allSupportedCultures.Add(libConfiguration.DefaultCulture);
            }

            foreach (var scopeDirectory in scopeDirectories)
            {
                foreach (var supportedCulture in allSupportedCultures)
                {
                    var currentResourceFileFound = false;

                    foreach (var fileExtension in m_dictionaryFactory.FileExtensions)
                    {
                        var currentPath = ConstructResourceFileName(libConfiguration, scopeDirectory, supportedCulture,
                            fileExtension);

                        if (File.Exists(currentPath))
                        {
                            currentResourceFileFound = true;
                            localizationFiles.Add(currentPath);

                            break;
                        }
                    }

                    if (!currentResourceFileFound)
                    {
                        var message = string.Format(
                            @"Dictionary file ""{0}[{1}]"" is missing.",
                            ConstructResourceFileName(libConfiguration, scopeDirectory, supportedCulture, ""),
                            string.Join("|", m_dictionaryFactory.FileExtensions)
                        );
                        if (m_logger != null && m_logger.IsInformationEnabled())
                        {
                            m_logger.LogInformation(message);
                        }

                        // do not throw errors if a culture is missing to allow for dynamic user-created dictionaries
                        //exceptionLogStringBuilder.AppendLine(message);
                        //shouldThrowException = true;
                    }
                }
            }

            foreach (var supportedCulture in allSupportedCultures)
            {
                var globalPathWithoutExtension = Path.Combine(libConfiguration.BasePath, supportedCulture.Name);
                var globalResourceFileFound = false;

                foreach (var fileExtension in m_dictionaryFactory.FileExtensions)
                {
                    var globalPath = string.Concat(globalPathWithoutExtension, ".", fileExtension);

                    if (File.Exists(globalPath))
                    {
                        globalResourceFileFound = true;
                        localizationFiles.Add(globalPath);

                        break;
                    }
                }

                if (!globalResourceFileFound)
                {
                    var message = string.Format(
                        @"Cannot init library. Dictionary file ""{0}.[{1}]"" is missing.",
                        globalPathWithoutExtension,
                        string.Join("|", m_dictionaryFactory.FileExtensions)
                    );
                    if (m_logger != null && m_logger.IsInformationEnabled())
                    {
                        m_logger.LogInformation(message);
                    }

                    // do not throw errors if a culture is missing to allow for dynamic user-created dictionaries
                    //exceptionLogStringBuilder.AppendLine(message);
                    //shouldThrowException = true;
                }
            }

            if (shouldThrowException)
            {
                throw new DictionaryLoadException(string.Concat("Missing resource file/s.", '\n',
                    exceptionLogStringBuilder.ToString()));
            }

            return localizationFiles;
        }

        /// <summary>
        /// Check for resource files base on folders structure in basePath.
        /// </summary>
        /// <param name="libConfiguration">Library configuration.</param>
        public IList<string> CheckResourceFiles(LocalizationConfiguration libConfiguration)
        {
            return CheckScopeResourceFiles(libConfiguration, ScanScopeDirectories(libConfiguration.BasePath))
                .Union(CheckGlobalResourceFiles(libConfiguration)).ToList();
        }

        /// <summary>
        /// Creates dictionary filename by provided directory
        /// </summary>
        /// <param name="libConfiguration"></param>
        /// <param name="directory"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public string ConstructResourceFileName(LocalizationConfiguration libConfiguration, string directory,
            CultureInfo cultureInfo, string fileExtension)
        {
            var cutString = string.Concat(libConfiguration.BasePath, Path.DirectorySeparatorChar);
            var nameBase = directory.Split(new[] {cutString}, StringSplitOptions.None);
            if (nameBase.Length != 2)
            {
                var message = string.Format(@"Provided path ""{0}"" is not inside basepath: ""{1}""", directory,
                    libConfiguration.BasePath);
                if (m_logger != null && m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(message);
                }

                throw new DictionaryLoadException(message);
            }

            var dotNotatedFileName = nameBase[1].Replace(Path.DirectorySeparatorChar, '.');
            dotNotatedFileName = string.Concat(dotNotatedFileName, '.', cultureInfo.Name, '.', fileExtension);

            return Path.Combine(directory, dotNotatedFileName);
        }
    }
}
