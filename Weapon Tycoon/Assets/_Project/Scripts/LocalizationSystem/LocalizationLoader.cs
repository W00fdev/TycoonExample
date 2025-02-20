using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace _Project.Scripts.LocalizationSystem
{
    public class LocalizationLoader
    {
        private const string _path = "Localization";
        private const char _splitSymbol = ';';

        private Dictionary<string, int> _languagesData = new();
        private Dictionary<string, string[]> _localizedWords = new();

        public IReadOnlyDictionary<string, int> LanguageData => _languagesData;
        public IReadOnlyDictionary<string, string[]> LocalizedWords => _localizedWords;
        
        public LocalizationLoader() { }

        public void Load()
        {
            var csvLines = ReadCsvFile();

            _languagesData = GetLanguagesData(csvLines);
            _localizedWords = GetLocalizedWord(csvLines);
        }

        public string[] ReadCsvFile()
        {
            var myText = Resources.Load<TextAsset>(_path);
            var text = myText.text;
            List<string> lines = new();

            using StringReader reader = new(text);
            string line;
            while ((line = reader.ReadLine()) != null)
                lines.Add(line);
            
            return lines.ToArray();
        }

        public Dictionary<string, int> GetLanguagesData(string[] csvLines)
        {
            Dictionary<string, int> languagesData = new();
            
            var languageData = csvLines[0].Split(_splitSymbol);
            for(int i = 1; i < languageData.Length; i++)
            {
                var language = languageData[i];
                languagesData.Add(language, i - 1);
            }

            return languagesData;
        }

        public Dictionary<string, string[]> GetLocalizedWord(string[] csvLines)
        {
            Dictionary<string, string[]> localizedWord = new();
            
            for (int i = 1; i < csvLines.Length; i++)
            {
                List<string> lineData = csvLines[i].Split(_splitSymbol).ToList();
                string key = lineData[0];
                lineData.RemoveAt(0);
                localizedWord.Add(key, lineData.ToArray());
            }

            return localizedWord;
        }
    }
}