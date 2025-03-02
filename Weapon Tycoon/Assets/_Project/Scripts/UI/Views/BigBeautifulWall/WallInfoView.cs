using _Project.Scripts.LocalizationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI.Views.BigBeautifulWall
{
    public class WallInfoView : MonoBehaviour
    {
        [SerializeField] private LangText _name;
        [SerializeField] private TMP_Text _regenText;
        [SerializeField] private TMP_Text _healthText;

        public void Initialize(string keyName)
            => _name.ChangeKey(keyName);

        public void UpdateInfo(string regen, string health)
        {
            _regenText.text = regen;
            _healthText.text = health;
        }
    }
}