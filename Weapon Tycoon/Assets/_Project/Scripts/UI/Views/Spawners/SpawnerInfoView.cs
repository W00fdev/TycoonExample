using _Project.Scripts.LocalizationSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Views.Spawners
{
    public class SpawnerInfoView : MonoBehaviour
    {
        [SerializeField] private LangText _name;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _priceText;

        public void Initialize(string keyName)
            => _name.ChangeKey(keyName);

        public void UpdateInfo(string speed, string price)
        {
            _speedText.text = speed;
            _priceText.text = price;
        }
    }
}