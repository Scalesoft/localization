using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Scalesoft.Localization.Tool.Translator.Models;
using Scalesoft.Localization.Tool.Translator.Models.Json;

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
    }
}
