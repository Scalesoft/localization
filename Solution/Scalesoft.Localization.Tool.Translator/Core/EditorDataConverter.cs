using System.Collections.Generic;
using System.Linq;
using Scalesoft.Localization.Tool.Translator.Models;

namespace Scalesoft.Localization.Tool.Translator.Core
{
    public class EditorDataConverter
    {
        public EditorViewModel ConvertToViewModel(ScopeViewModel scope)
        {
            var standardRows = new List<EditorRowViewModel>();
            var constantRows = new List<EditorRowViewModel>();

            var result = new EditorViewModel
            {
                Scope = scope.Scope,
                Cultures = scope.Dictionaries.Select(x => x.DictionaryData.Culture).ToArray(),
                Files = scope.Dictionaries.Select(x => x.FileInfo).ToArray(),
                StandardRows = standardRows,
                ConstantRows = constantRows,
                Pluralizations = new dynamic[scope.Dictionaries.Count],
            };

            var index = 0;
            foreach (var dictionaryEnvelopeViewModel in scope.Dictionaries)
            {
                ConvertDictionary(dictionaryEnvelopeViewModel.DictionaryData.Dictionary, standardRows, index, result.Cultures.Length);
                ConvertDictionary(dictionaryEnvelopeViewModel.DictionaryData.Constants, constantRows, index, result.Cultures.Length);

                result.Pluralizations[index] = dictionaryEnvelopeViewModel.DictionaryData.Plural;
                
                index++;
            }

            return result;
        }

        private void ConvertDictionary(IDictionary<string, string> dictionary, IList<EditorRowViewModel> resultRows, int index, int cultureCount)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (var item in dictionary)
            {
                var row = resultRows.FirstOrDefault(x => x.Key == item.Key);
                if (row == null)
                {
                    row = new EditorRowViewModel
                    {
                        Key = item.Key,
                        Translations = new string[cultureCount],
                    };
                    resultRows.Add(row);
                }

                row.Translations[index] = item.Value;
            }
        }
    }
}