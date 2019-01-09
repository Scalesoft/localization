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

            var defaultCultureResourceFilePath = Path.Combine(libConfiguration.BasePath, defaultCulture.Name);
            defaultCultureResourceFilePath = string.Concat(defaultCultureResourceFilePath, ".",
                m_dictionaryFactory.FileExtension);

            if (File.Exists(defaultCultureResourceFilePath))
            {
                localizationFiles.Add(defaultCultureResourceFilePath);
            }
            else
            {
                var message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                    defaultCultureResourceFilePath);
                if (m_logger != null && m_logger.IsErrorEnabled())
                {
                    m_logger.LogError(message);
                }

                exceptionLogStringBuilder.AppendLine(message);
                shouldThrowException = true;
            }

            foreach (var supportedCulture in libConfiguration.SupportedCultures)
            {
                var supportedFilePathWithoutExtension =
                    Path.Combine(libConfiguration.BasePath, supportedCulture.Name);
                var supportedcultureFilePath = string.Concat(supportedFilePathWithoutExtension, ".",
                    m_dictionaryFactory.FileExtension);
                if (File.Exists(supportedcultureFilePath))
                {
                    localizationFiles.Add(supportedcultureFilePath);
                }
                else
                {
                    var message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                        supportedcultureFilePath);
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(message);
                    }

                    exceptionLogStringBuilder.AppendLine(message);
                    shouldThrowException = true;
                }
            }

            if (shouldThrowException)
            {
                throw new DictionaryLoadException(string.Concat("Missing resource file/s.", '\n', exceptionLogStringBuilder.ToString()));
            }

            return localizationFiles;
        }

        /// <summary>
        /// Check for resource files in provided folders.
        /// Each folder represents scope. Scopes can be nested "infinitely".
        /// </summary>
        /// <param name="libConfiguration">Library configuration.</param>
        /// <param name="scopeDirectories">Array with all folders inside localization folder</param>
        private IEnumerable<string> CheckScopeResourceFiles(LocalizationConfiguration libConfiguration, string[] scopeDirectories)
        {
            IList<string> localizationFiles = new List<string>();

            var exceptionLogStringBuilder = new StringBuilder();
            var shouldThrowException = false;
            foreach (var scopeDirectory in scopeDirectories)
            {
                foreach (var supportedCulture in libConfiguration.SupportedCultures)
                {
                    var currentPath = ConstructResourceFileName(libConfiguration, scopeDirectory, supportedCulture);
                    if (File.Exists(currentPath))
                    {
                        localizationFiles.Add(currentPath);
                    }
                    else
                    {
                        var message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                            currentPath);
                        if (m_logger != null && m_logger.IsErrorEnabled())
                        {
                            m_logger.LogError(message);
                        }

                        exceptionLogStringBuilder.AppendLine(message);
                        shouldThrowException = true;
                    }
                }

                var defaultPath = ConstructResourceFileName(libConfiguration, scopeDirectory, libConfiguration.DefaultCulture);
                if (File.Exists(defaultPath))
                {
                    localizationFiles.Add(defaultPath);
                }
                else
                {
                    var message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                        defaultPath);
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(message);
                    }

                    exceptionLogStringBuilder.AppendLine(message);
                    shouldThrowException = true;
                }
            }

            foreach (var supportedCulture in libConfiguration.SupportedCultures)
            {
                var globalPath = string.Concat(supportedCulture.Name, ".", m_dictionaryFactory.FileExtension);
                globalPath = Path.Combine(libConfiguration.BasePath, globalPath);
                if (File.Exists(globalPath))
                {
                    localizationFiles.Add(globalPath);
                }
                else
                {
                    var message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.", globalPath);
                    if (m_logger != null && m_logger.IsErrorEnabled())
                    {
                        m_logger.LogError(message);
                    }

                    exceptionLogStringBuilder.AppendLine(message);
                    shouldThrowException = true;
                }
            }

            if (shouldThrowException)
            {
                throw new DictionaryLoadException(string.Concat("Missing resource file/s.", '\n', exceptionLogStringBuilder.ToString()));
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
        public string ConstructResourceFileName(LocalizationConfiguration libConfiguration, string directory, CultureInfo cultureInfo)
        {
            var cutString = string.Concat(libConfiguration.BasePath, Path.DirectorySeparatorChar);
            var nameBase = directory.Split(new string[] {cutString}, StringSplitOptions.None);
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
            dotNotatedFileName = string.Concat(dotNotatedFileName, '.', cultureInfo.Name, '.', m_dictionaryFactory.FileExtension);

            return Path.Combine(directory, dotNotatedFileName);
        }
    }
}
