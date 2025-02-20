using System;
using UnityEngine;
using TMPro;

namespace _Project.Scripts.LocalizationSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LangText : MonoBehaviour
    {
        [SerializeField] private string _key;

        private TextMeshProUGUI _text;
        private ITranslator _translator;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(_key))
                _key = _key.ToLower();
        }
#endif
        
        private void Awake()
        {
            _text ??= GetComponent<TextMeshProUGUI>();
            Localization.LanguageChanged += OnLanguageChanged;
        }

        //private void OnEnable() => OnLanguageChanged();

        private void Start() => OnLanguageChanged();

        private void OnDestroy() => Localization.LanguageChanged -= OnLanguageChanged;

        public void SetKey(string key) => _key = key;

        public void ChangeKey(string key)
        {
            SetKey(key);
            OnLanguageChanged();
        }

        private void OnLanguageChanged()
        {
            _translator ??= Localization.Instance;
            _text ??= GetComponent<TextMeshProUGUI>();
            
            if (_text != null && _translator != null)
                _text.text = _translator.GetTranslate(_key);
        }
    }
}