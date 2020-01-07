using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Scalesoft.Localization.Tool.Translator.Models;
using Scalesoft.Localization.Tool.Translator.Models.Json;
using Scalesoft.Localization.Tool.Translator.Models.Requests;

namespace Scalesoft.Localization.Tool.Translator.Core
{
    public class DictionaryManager
    {
        private readonly IWebHostEnvironment m_webHostEnvironment;
        private const string WorkingDirectory = "WorkingDirectory";

        public DictionaryManager(IWebHostEnvironment webHostEnvironment)
        {
            m_webHostEnvironment = webHostEnvironment;
        }

        public IList<DictionaryEnvelopeViewModel> GetDictionaries()
        {
            var dirPath = Path.Combine(m_webHostEnvironment.ContentRootPath, WorkingDirectory);
            var directory = new DirectoryInfo(dirPath);

            var resultList = new List<DictionaryEnvelopeViewModel>();

            LoadDictionaries(directory, resultList);

            return resultList;
        }

        private void LoadDictionaries(DirectoryInfo directory, List<DictionaryEnvelopeViewModel> resultList)
        {
            foreach (var fileInfo in directory.EnumerateFiles())
            {
                using (var stream = fileInfo.OpenRead())
                using (var reader = new StreamReader(stream))
                {
                    var fileContent = reader.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<DictionaryModel>(fileContent);

                    resultList.Add(new DictionaryEnvelopeViewModel
                    {
                        FileInfo = fileInfo,
                        DictionaryData = data,
                    });
                }
            }

            foreach (var directoryInfo in directory.EnumerateDirectories())
            {
                LoadDictionaries(directoryInfo, resultList);
            }
        }

        public IList<ScopeViewModel> GetData(string scope = null)
        {
            var dictionaries = GetDictionaries().AsEnumerable();
            var resultList = new List<ScopeViewModel>();

            if (scope != null)
            {
                dictionaries = dictionaries.Where(x => x.DictionaryData.Scope == scope);
            }

            foreach (var dictionaryEnvelopeViewModel in dictionaries)
            {
                var scopeModel = resultList.FirstOrDefault(x => x.Scope == dictionaryEnvelopeViewModel.DictionaryData.Scope);
                if (scopeModel == null)
                {
                    scopeModel = new ScopeViewModel
                    {
                        Scope = dictionaryEnvelopeViewModel.DictionaryData.Scope,
                        Dictionaries = new List<DictionaryEnvelopeViewModel>(),
                    };
                    resultList.Add(scopeModel);
                }

                scopeModel.Dictionaries.Add(dictionaryEnvelopeViewModel);
            }

            return resultList;
        }

        public ScopeViewModel GetDataForScope(string scope)
        {
            return GetData(scope).SingleOrDefault();
        }

        public void CreateDictionariesForCulture(string culture)
        {
            var scopes = GetData();

            foreach (var scopeViewModel in scopes)
            {
                var dictionary = scopeViewModel.Dictionaries[0];
                
                // Content
                var model = new DictionaryModel
                {
                    Culture = culture,
                    Scope = scopeViewModel.Scope,
                };
                var fileContent = JsonConvert.SerializeObject(model, Formatting.Indented);

                // Path
                var directory = dictionary.FileInfo.Directory;
                var fileName = dictionary.FileInfo.Name;
                var extension = dictionary.FileInfo.Extension;
                var originalCulture = dictionary.DictionaryData.Culture;
                var cultureIndex = fileName.LastIndexOf(originalCulture, StringComparison.Ordinal);

                fileName = fileName.Substring(0, cultureIndex);
                fileName = $"{fileName}{culture}{extension}";

                // Write file
                Debug.Assert(directory != null, nameof(directory) + " != null");
                var newFileFullPath = Path.Combine(directory.FullName, fileName);
                using (var fileStream = File.Create(newFileFullPath))
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(fileContent);
                }
            }
        }

        public void SaveDictionaryChanges(SaveLocalizationRequest request)
        {
            var scopes = GetDataForScope(request.Scope);
            foreach (var dictionaryEnvelopeViewModel in scopes.Dictionaries)
            {
                var anyChange = false;
                var model = dictionaryEnvelopeViewModel.DictionaryData;
                var changesForCulture1 = request.Dictionary.Where(x => x.Culture == model.Culture);
                var changesForCulture2 = request.Constants.Where(x => x.Culture == model.Culture);

                foreach (var itemChangeRequest in changesForCulture1)
                {
                    if (model.Dictionary == null)
                    {
                        model.Dictionary = new Dictionary<string, string>();
                    }

                    model.Dictionary[itemChangeRequest.Key] = itemChangeRequest.Value;
                    anyChange = true;
                }

                foreach (var itemChangeRequest in changesForCulture2)
                {
                    if (model.Constants == null)
                    {
                        model.Constants = new Dictionary<string, string>();
                    }

                    model.Constants[itemChangeRequest.Key] = itemChangeRequest.Value;
                    anyChange = true;
                }

                if (anyChange)
                {
                    var serializedContent = JsonConvert.SerializeObject(model, Formatting.Indented);

                    File.WriteAllText(dictionaryEnvelopeViewModel.FileInfo.FullName, serializedContent);
                }
            }
        }
    }
}
