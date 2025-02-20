using System;


namespace _Project.Scripts.LocalizationSystem
{
    public class Localization : ITranslator, ILanguageChanger
    {
        public static event Action LanguageChanged;
        private string _currentLanguage = "EN";

        private readonly LocalizationLoader _localizationLoader;

        private static Localization _instance;
        public static Localization Instance
        {
            get => _instance;
            private set => _instance = value;
        }

        public Localization(LocalizationLoader localizationLoader)
        {
            _localizationLoader = localizationLoader;
            _instance = this;
        }

        public void SetLanguage(string language)
        {
            if (!_localizationLoader.LanguageData.ContainsKey(language))
                return;

            _currentLanguage = language;

            LanguageChanged?.Invoke();
        }

        public string GetTranslate(string key)
        {
            key = key.ToLower();
            string text = key;

            if (!_localizationLoader.LocalizedWords.ContainsKey(key))
                return text;

            var currentLanguageId = _localizationLoader.LanguageData[_currentLanguage];
            text = _localizationLoader.LocalizedWords[key][currentLanguageId];

            return text;
        }
    }
}