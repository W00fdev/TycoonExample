using UnityEngine;

namespace _Project.Scripts.LocalizationSystem
{
    public class LanguageDetector
    {
        private readonly ILanguageChanger _languageChanger;

        public LanguageDetector(ILanguageChanger languageChanger) =>
            _languageChanger = languageChanger;

        public void DetectSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    _languageChanger.SetLanguage("RU");
                    break;

                default:
                    _languageChanger.SetLanguage("EN");
                    break;
            }
        }

        public void ProcessBrowserLanguage(string language)
        {
            switch (language)
            {
                case "ru":
                    _languageChanger.SetLanguage("RU");
                    break;
                default:
                    _languageChanger.SetLanguage("EN");
                    break;
            }
        }
    }
}