using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Project.Scripts.LocalizationSystem
{
    public class LocalizationChangeAdapter : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _langKey;

        private void OnEnable()
        {
            _button.onClick.AddListener(ChangeLanguage);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ChangeLanguage);
        }

        private void ChangeLanguage()
        {
            Localization.Instance.SetLanguage(_langKey);
        }
    }
}