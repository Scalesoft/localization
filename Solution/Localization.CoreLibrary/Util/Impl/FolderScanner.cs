using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Localization.CoreLibrary.Dictionary;
using Localization.CoreLibrary.Logging;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Util.Impl
{
    public class FolderScanner : ILocalizationStructureScanner
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();
        private readonly IDictionaryFactory m_dictionaryFactory;

        public FolderScanner(IDictionaryFactory dictionaryFactory)
        {
            m_dictionaryFactory = dictionaryFactory;
        }

        /// <summary>
        /// Scans recursively folders inside basePath.
        /// </summary>
        /// <param name="basePath">FileLocalization base path.</param>
        /// <returns>array containing all scope folders.</returns>
        private static string[] ScanScopeDirectories(string basePath)
        {
            return Directory.GetDirectories(basePath);
        }

        private IEnumerable<string> CheckGlobalResourceFiles(IConfiguration libConfiguration)
        {
            IList<string> localizationFiles = new List<string>();
            StringBuilder exceptionLogStringBuilder = new StringBuilder();
            bool shouldThrowException = false;

            CultureInfo defaultCulture = libConfiguration.DefaultCulture();

            string defaultCultureResourceFilePath = Path.Combine(libConfiguration.BasePath(), defaultCulture.Name);
            defaultCultureResourceFilePath = string.Concat(defaultCultureResourceFilePath, ".",
                m_dictionaryFactory.FileExtension);

            if (File.Exists(defaultCultureResourceFilePath))
            {
                localizationFiles.Add(defaultCultureResourceFilePath);
            }
            else
            {
                string message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                    defaultCultureResourceFilePath);
                Logger.LogError(message);
                exceptionLogStringBuilder.AppendLine(message);
                shouldThrowException = true;
            }

            foreach (CultureInfo supportedCulture in libConfiguration.SupportedCultures())
            {
                string supportedFilePathWithoutExtension =
                    Path.Combine(libConfiguration.BasePath(), supportedCulture.Name);
                string supportedcultureFilePath = string.Concat(supportedFilePathWithoutExtension, ".",
                    m_dictionaryFactory.FileExtension);
                if (File.Exists(supportedcultureFilePath))
                {
                    localizationFiles.Add(supportedcultureFilePath);
                }
                else
                {
                    string message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                        supportedcultureFilePath);
                    Logger.LogError(message);
                    exceptionLogStringBuilder.AppendLine(message);
                    shouldThrowException = true;
                }
            }

            if (shouldThrowException)
            {
                throw new System.Exception(string.Concat("Missing resource file/s.", '\n', exceptionLogStringBuilder.ToString()));
            }

            return localizationFiles;
        }

        /// <summary>
        /// Check for resource files in provided folders.
        /// Each folder represents scope. Scopes can be nested "infinitely".
        /// </summary>
        /// <param name="libConfiguration">Library configuration.</param>
        /// <param name="scopeDirectories">Array with all folders inside localization folder</param>
        private IEnumerable<string> CheckScopeResourceFiles(IConfiguration libConfiguration, string[] scopeDirectories)
        {
            IList<string> localizationFiles = new List<string>();

            StringBuilder exceptionLogStringBuilder = new StringBuilder();
            bool shouldThrowException = false;
            foreach (string scopeDirectory in scopeDirectories)
            {
                foreach (CultureInfo supportedCulture in libConfiguration.SupportedCultures())
                {
                    string currentPath = ConstructResourceFileName(libConfiguration, scopeDirectory, supportedCulture);
                    if (File.Exists(currentPath))
                    {
                        localizationFiles.Add(currentPath);
                    }
                    else
                    {
                        string message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                            currentPath);
                        Logger.LogError(message);
                        exceptionLogStringBuilder.AppendLine(message);
                        shouldThrowException = true;
                    }
                }

                string defaultPath = ConstructResourceFileName(libConfiguration, scopeDirectory, libConfiguration.DefaultCulture());
                if (File.Exists(defaultPath))
                {
                    localizationFiles.Add(defaultPath);
                }
                else
                {
                    string message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.",
                        defaultPath);
                    Logger.LogError(message);
                    exceptionLogStringBuilder.AppendLine(message);
                    shouldThrowException = true;
                }
            }
            foreach (CultureInfo supportedCulture in libConfiguration.SupportedCultures())
            {
                string globalPath = string.Concat(supportedCulture.Name, ".", m_dictionaryFactory.FileExtension);
                globalPath = Path.Combine(libConfiguration.BasePath(), globalPath);
                if (File.Exists(globalPath))
                {
                    localizationFiles.Add(globalPath);
                }
                else
                {
                    string message = string.Format(@"Cannot init library. Dictionary file ""{0}"" is missing.", globalPath);
                    Logger.LogError(message);
                    exceptionLogStringBuilder.AppendLine(message);
                    shouldThrowException = true;
                }
            }          
            if (shouldThrowException)
            {
                throw new System.Exception(string.Concat("Missing resource file/s.", '\n', exceptionLogStringBuilder.ToString()));
            }

            return localizationFiles;
        }

        /// <summary>
        /// Check for resource files base on folders structure in basePath.
        /// </summary>
        /// <param name="libConfiguration">Library configuration.</param>
        public IList<string> CheckResourceFiles(IConfiguration libConfiguration)
        {
            return CheckScopeResourceFiles(libConfiguration, ScanScopeDirectories(libConfiguration.BasePath())).Union(CheckGlobalResourceFiles(libConfiguration)).ToList();
        }

        /// <summary>
        /// Creates dictionary filename by provided directory
        /// </summary>
        /// <param name="libConfiguration"></param>
        /// <param name="directory"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public string ConstructResourceFileName(IConfiguration libConfiguration, string directory, CultureInfo cultureInfo)
        {
            string cutString = string.Concat(libConfiguration.BasePath(), Path.DirectorySeparatorChar);
            string[] nameBase = directory.Split(new string[] { cutString }, StringSplitOptions.None);
            if (nameBase.Length != 2)
            {
                string message = string.Format(@"Provided path ""{0}"" is not inside basepath: ""{1}""", directory,
                    libConfiguration.BasePath());
                Logger.LogError(message);
                throw new System.Exception(message);
            }
            string dotNotatedFileName = nameBase[1].Replace(Path.DirectorySeparatorChar, '.');           
            dotNotatedFileName = string.Concat(dotNotatedFileName, '.', cultureInfo.Name, '.', m_dictionaryFactory.FileExtension);

            return Path.Combine(directory, dotNotatedFileName);
        }
    }
}