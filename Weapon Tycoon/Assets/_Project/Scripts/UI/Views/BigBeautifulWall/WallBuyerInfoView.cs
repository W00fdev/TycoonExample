using _Project.Scripts.LocalizationSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.UI.Views.BigBeautifulWall
{
    public class WallBuyerInfoView : MonoBehaviour
    {
        [SerializeField] private LangText _name;
        [SerializeField] private TMP_Text _regenText;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _wallPriceText;

        public void Initialize(string wallKeyName, string wallPrice, string regen, string health)
        {
            _name.ChangeKey(wallKeyName);
            
            _wallPriceText.text = wallPrice;
            _healthText.text = health;
            _regenText.text = regen;
        }

        public void DisableSelf()
            => gameObject.SetActive(false);
    }
}